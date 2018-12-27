
using UnityEngine;
using StateMachine;

public class chaseState : State<StateMachineAI>
{

    //Make the attack state a singleton to ensure there is only ever one at any time
    private static chaseState _instance;

    private chaseState()
    {
        if (_instance != null)
        {
            return; //Simply return if there is already an instance of attack
        }

        _instance = this; //set instance to this instance if there isn't already an instance
    }

    public static chaseState Instance
    {
        get
        {
            if (_instance == null)
            {
                new chaseState(); //Create a new instance of attack or return existing
            }

            return _instance;
        }
    }
    //End of singleton implementation

    public override void EnterState(StateMachineAI bot)
    {
        bot.setTarget(bot.GetClosestObject(bot.getSenses().GetEnemiesInView()));
    }

    public override void ExitState(StateMachineAI bot)
    {
        
    }

    public override void UpdateState(StateMachineAI bot)
    {
        bot.chase();
    }
}
