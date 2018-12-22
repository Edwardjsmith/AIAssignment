using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StateMachineAI : AI
{

    // Use this for initialization
    public override void Start ()
    {
        base.Start();
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();
	}

    public override void idleState()
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

    public override void pickupFlag()
    {
        if (getSenses().IsItemInReach(target))
        {
            getActions().CollectItem(target);
            StateMachine.transitionToNewState(runFlagToBaseState.Instance);
        }
    }
    public override void pickupItem()
    {
        if (getSenses().IsItemInReach(target))
        {
            getActions().CollectItem(target);
            StateMachine.transitionToNewState(searchForFlagState.Instance);
        }
    }
    public override void attack()
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

    public override void chase()
    {
        if (target == null)
        {
            StateMachine.transitionToNewState(searchForFlagState.Instance);
        }
        else
        {
            getActions().MoveTo(target);
            if(powerup != null && GetInventory().HasItem(powerup.name))
            {
                getActions().UseItem(powerup);
            }
            
            if (Vector3.Distance(transform.position, getTargetObj().transform.position) <= getData().AttackRange)
            {
                StateMachine.transitionToNewState(attackState.Instance);
            }
        }

    }

    public override void findHealth()
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

    public override void runFlagToBase()
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


    public override void flee()
    {
        base.flee();
        if (target != null)
        { 
            if (fleeTimer > 0)
            {
                getActions().Flee(target);
                fleeTimer -= Time.deltaTime;
            }
        }
        else
        {
            fleeTimer = 10.0f;
            StateMachine.transitionToNewState(findHealthState.Instance);
        }

    }

    public override void saveFlag()
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
    public override void defendFlag()
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
