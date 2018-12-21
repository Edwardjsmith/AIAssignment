using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StateMachineAI : AI
{
    public enum flagToTake { Blue, Red};
    public flagToTake flag;
    GameObject enemyFlag;

    public enum myBase { Blue, Red };
    public myBase baseEnum;
    GameObject Base;
    bool randomPosSet = false;
    NavMeshAgent agent;
    

    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        StateMachine.transitionToNewState(searchForFlagState.Instance);
        enemyFlag = GameObject.Find(flag.ToString() + " Flag");
        Base = GameObject.Find(baseEnum.ToString() + " Base");
        agent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();
	}

    public override void searchForFlag()
    {
        base.searchForFlag();

        if (getTargetObj() == null && !randomPosSet)
        {
            getActions().MoveToRandomLocation();
            randomPosSet = true;
        }
        if (agent.velocity == Vector3.zero)
        {
            randomPosSet = false;
        }


        //if(getSenses().GetEnemiesInView() != null)
        {
            //Pick closest enemy
            //target = GetClosestObject(getSenses().GetEnemiesInView());
            //StateMachine.transitionToNewState(chaseState.Instance);
        }

        if(getSenses().GetObjectInViewByName(enemyFlag.name))
        {
            target = enemyFlag;
            getActions().MoveTo(target);

            if(getSenses().IsItemInReach(target))
            {
                getActions().CollectItem(target);
                target = Base;
                StateMachine.transitionToNewState(runFlagToBaseState.Instance);
            }
        }

    }

    public override void attack()
    {
        base.attack();

        if(getData().CurrentHitPoints < getData().CurrentHitPoints / 2)
        {
            StateMachine.transitionToNewState(fleeState.Instance);
        }

        if (Vector3.Distance(transform.position, getTargetObj().transform.position) >= getData().AttackRange)
        {
            getActions().AttackEnemy(getTargetObj());
        }
        else
        {
            StateMachine.transitionToNewState(chaseState.Instance);
        }
    }

    public override void chase()
    {
        base.chase();

        getActions().MoveTo(target);

        if (getData().CurrentHitPoints < getData().CurrentHitPoints / 2)
        {
            StateMachine.transitionToNewState(fleeState.Instance);
        }
        //if (Vector3.Distance(transform.position, getTargetObj().transform.position) <= getData().AttackRange)
        {
           //StateMachine.transitionToNewState(attackState.Instance);
        }


    }

    public override void findHealth()
    {
        base.findHealth();

        if (getTargetObj() == null)
        {
            getActions().MoveToRandomLocation();
        }

        if (getSenses().GetObjectInViewByName("Health Kit"))
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
        base.runFlagToBase();
        getActions().MoveTo(target);

        if(Vector3.Distance(transform.position, target.transform.position) < 3)
        {
            getActions().DropAllItems();
            StateMachine.transitionToNewState(searchForFlagState.Instance);
        }
    }

    public override void flee()
    {
        base.flee();
        getActions().Flee(target);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall")
        { 
            randomPosSet = false;
        }
    }
}
