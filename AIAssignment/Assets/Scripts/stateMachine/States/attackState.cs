using StateMachine;
using UnityEngine;

public class attackState : State<StateMachineAI>
{
    //Make the attack state a singleton to ensure there is only ever one at any time
    private static attackState _instance;

    private attackState()
    {
        if (_instance != null)
        {
            return; //Simply return if there is already an instance of attack
        }

        _instance = this; //set instance to this instance if there isn't already an instance
    }

    public static attackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new attackState(); //Create a new instance of attack or return existing
            }

            return _instance; 
        }
    }
    //End of singleton implementation

    public override void EnterState(StateMachineAI bot)
    {
        bot.agent.speed = 0;
    }

    public override void ExitState(StateMachineAI bot)
    {
        bot.agent.speed = 5;
    }

    public override void UpdateState(StateMachineAI bot)
    {
        bot.attack();
    }
}
