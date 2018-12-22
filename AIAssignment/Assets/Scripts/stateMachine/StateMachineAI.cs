using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StateMachineAI : AI
{
    public enum flagToTake { Blue, Red};
    public flagToTake flag;
    GameObject enemyFlag;
    GameObject friendlyFlag;

    public enum myBase { Blue, Red };
    public myBase baseEnum;
    GameObject Base;
    bool randomPosSet = false;
    

    bool defendFlagPosSet = false;

    public delegate bool queryFlag();
    public delegate bool setFlag();

    static bool blueFlagCaptured;
    static bool redFlagCaptured;

    queryFlag flagAtBase; 
    setFlag flagtaken;

    float fleeTimer = 10.0f;
    
    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        StateMachine.transitionToNewState(searchForFlagState.Instance);
        enemyFlag = GameObject.Find(flag.ToString() + " Flag");
        friendlyFlag = GameObject.Find(getData().FriendlyFlagName);
        Base = GameObject.Find(baseEnum.ToString() + " Base");
        agent = GetComponent<NavMeshAgent>();

        flagAtBase += enemyFlagAtBase;
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();
	}

    public override void searchForFlag()
    {
        base.searchForFlag();
        
        if (baseEnum == myBase.Blue)
        {
            if(blueFlagCaptured)
            {
                StateMachine.transitionToNewState(saveFlagState.Instance);
            }
        }
        else
        {
            if(redFlagCaptured)
            {
                StateMachine.transitionToNewState(saveFlagState.Instance);
            }
        }
        if (getData().CurrentHitPoints < getData().CurrentHitPoints / 2)
        {
            StateMachine.transitionToNewState(findHealthState.Instance);
        }

        if (getTargetObj() == null && !randomPosSet)
        {
            getActions().MoveToRandomLocation();
            randomPosSet = true;
        }
        if (agent.velocity == Vector3.zero)
        {
            randomPosSet = false;
        }


        if(getSenses().GetEnemiesInView() != null)
        {
            //Pick closest enemy
            target = GetClosestObject(getSenses().GetEnemiesInView());
            if (target != null)
            {
                StateMachine.transitionToNewState(chaseState.Instance);
            }
        }

        if(getSenses().GetObjectInViewByName(enemyFlag.name) && !flagAtBase())
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

        if (getTargetObj() != null)
        {
            if (Vector3.Distance(transform.position, getTargetObj().transform.position) <= getData().AttackRange)
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
        base.chase();

        if (target != null)
        {
            getActions().MoveTo(target);
        }
        else
        {
            StateMachine.transitionToNewState(searchForFlagState.Instance);
        }

        if (getData().CurrentHitPoints < getData().CurrentHitPoints / 2)
        {
            StateMachine.transitionToNewState(fleeState.Instance);
        }
        if (target != null && Vector3.Distance(transform.position, getTargetObj().transform.position) <= getData().AttackRange)
        {
           StateMachine.transitionToNewState(attackState.Instance);
        }
        else
        {
            StateMachine.transitionToNewState(searchForFlagState.Instance);
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
            StateMachine.transitionToNewState(defendFlagState.Instance);
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
        base.saveFlag();

        target = friendlyFlag;

        getActions().MoveTo(target);

        if (getSenses().IsItemInReach(target))
        {
            getActions().CollectItem(target);
            target = Base;
            StateMachine.transitionToNewState(runFlagToBaseState.Instance);
        }
    }
    public override void defendFlag()
    {
        base.defendFlag();

        flagCaptured(true);
        if (!defendFlagPosSet)
        {
            getActions().MoveTo(Base.transform.GetChild(Random.Range(0, 3)).transform.position);
            defendFlagPosSet = true;
        }

        if (getSenses().GetEnemiesInView() != null)
        {
            //Pick closest enemy
            target = GetClosestObject(getSenses().GetEnemiesInView());
            StateMachine.transitionToNewState(chaseState.Instance);
        }


    }

    bool flagCaptured(bool trueOrFalse)
    {
        if (baseEnum == myBase.Blue)
        {
            return redFlagCaptured = trueOrFalse;
        }
        else
        {
            return blueFlagCaptured = trueOrFalse;
        }
    }

    bool enemyFlagAtBase()
    {
        if (baseEnum == myBase.Blue)
        {
            return redFlagCaptured;
        }
        else
        {
            return blueFlagCaptured;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall")
        { 
            randomPosSet = false;
        }
        if(collision.transform.tag == "Wall" && StateMachine.currentState == fleeState.Instance)
        {
            StateMachine.transitionToNewState(findHealthState.Instance);
        }

    }
}
