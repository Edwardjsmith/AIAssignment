using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


public class StateMachineAI : AI
{
    stateMachine<StateMachineAI> StateMachine { get; set; }
    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        StateMachine = new stateMachine<StateMachineAI>(this);
        StateMachine.transitionToNewState(searchForFlagState.Instance);
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        StateMachine.Update();
    }

    public void idleState()
    {
        if (getSenses().GetEnemiesInView() != null)
        {
            StateMachine.transitionToNewState(chaseState.Instance);
        }
        if (getSenses().GetObjectInViewByName(getEnemyFlagObj().name) && !enemyFlagAtBase())
        {
            StateMachine.transitionToNewState(pickupEnemyFlagState.Instance);
        }
        if (getSenses().GetObjectInViewByName("Power Up"))
        {
            StateMachine.transitionToNewState(pickupItemState.Instance);
        }
        if(enemyFlagAtBase())
        {
            StateMachine.transitionToNewState(defendFlagState.Instance);
        }

        if (baseEnum == myBase.Blue)
        {
            if (blueFlagCaptured)
            {
                StateMachine.transitionToNewState(saveFlagState.Instance);
            }
        }
        else
        {
            if (redFlagCaptured)
            {
                StateMachine.transitionToNewState(saveFlagState.Instance);
            }
        }
    }

    public void pickupFlag()
    {
        if (getSenses().IsItemInReach(target))
        {
            getActions().CollectItem(target);
            StateMachine.transitionToNewState(runFlagToBaseState.Instance);
        }
    }
    public void pickupItem()
    {
        if (getSenses().IsItemInReach(target))
        {
            getActions().CollectItem(target);
            StateMachine.transitionToNewState(searchForFlagState.Instance);
        }
    }
    public void attack()
    {
        if (getTargetObj() != null)
        {
            if (getData().CurrentHitPoints < getData().CurrentHitPoints / 2 || getTargetObj().GetComponent<AI>().getData().IsPoweredUp)
            {
                StateMachine.transitionToNewState(fleeState.Instance);
            }
            else if (Vector3.Distance(transform.position, getTargetObj().transform.position) <= getData().AttackRange)
            {
                getActions().AttackEnemy(getTargetObj());

                if (getTargetObj() == null)
                {
                    StateMachine.transitionToNewState(searchForFlagState.Instance);
                }
            }
            else
            {
                StateMachine.transitionToNewState(chaseState.Instance);
            }
        }
        else
        {
            StateMachine.transitionToNewState(searchForFlagState.Instance);
        }
    }

    public void chase()
    {
        if (target == null)
        {
            StateMachine.transitionToNewState(searchForFlagState.Instance);
        }
        else
        {
            getActions().MoveTo(target);
            if(getPowerUp() != null && GetInventory().HasItem(getPowerUp().name))
            {
                getActions().UseItem(getPowerUp());
            }
            
            if (Vector3.Distance(transform.position, getTargetObj().transform.position) <= getData().AttackRange)
            {
                StateMachine.transitionToNewState(attackState.Instance);
            }
        }

    }

    public void findHealth()
    {

        if (getTargetObj() == null)
        {
            getActions().MoveToRandomLocation();
        }
        else if(getSenses().GetObjectInViewByName("Health Kit"))
        {
            target = getSenses().GetObjectInViewByName("Health Kit");
            getActions().MoveTo(target);

            if (getSenses().IsItemInReach(target))
            {
                getActions().CollectItem(target);
                getActions().UseItem(target);

                if(getData().CurrentHitPoints > getData().CurrentHitPoints / 2)
                {
                    StateMachine.transitionToNewState(searchForFlagState.Instance);
                }
                else
                {
                    findHealth();
                }
            }
        }
    }

    public void runFlagToBase()
    {
        if(Vector3.Distance(transform.position, target.transform.position) < 5)
        {
            if (GetInventory().HasItem(getEnemyFlagObj().name))
            {
                getActions().DropItem(getEnemyFlagObj());
                StateMachine.transitionToNewState(defendFlagState.Instance);
            }
            else
            {
                getActions().DropItem(getFirendlyFlagObj());
                StateMachine.transitionToNewState(searchForFlagState.Instance);
            }
        }
    }


    public void flee()
    {
        if (fleeTimer > 0)
        {
            fleeTimer -= Time.deltaTime;
        }
        else
        {
            fleeTimer = 10.0f;

            if (getData().CurrentHitPoints < getData().CurrentHitPoints / 2)
            {
                StateMachine.transitionToNewState(findHealthState.Instance);
            }
            else
            {
                StateMachine.transitionToNewState(searchForFlagState.Instance);
            }
        }
    }

    public void saveFlag()
    {
        if (getSenses().IsItemInReach(target))
        { 
            getActions().CollectItem(target);
            StateMachine.transitionToNewState(runFlagToBaseState.Instance);
        }
        else if (getSenses().GetEnemiesInView() != null)
        {
            StateMachine.transitionToNewState(chaseState.Instance);
        }
    }
    public void defendFlag()
    {
        if (getSenses().GetEnemiesInView() != null)
        {
            //Pick closest enemy
            target = GetClosestObject(getSenses().GetEnemiesInView());
            StateMachine.transitionToNewState(chaseState.Instance);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall" || collision.transform.tag == getData().FriendlyTeamTag)
        { 
            randomPosSet = false;
        }
        if(collision.transform.tag == "Wall" && StateMachine.currentState == fleeState.Instance)
        {
            StateMachine.transitionToNewState(findHealthState.Instance);
        }

    }
}
