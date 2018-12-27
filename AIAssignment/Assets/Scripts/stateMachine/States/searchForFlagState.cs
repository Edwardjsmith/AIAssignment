using UnityEngine;
using StateMachine;

public class searchForFlagState : State<StateMachineAI>
{
    //Make the attack state a singleton to ensure there is only ever one at any time
    private static searchForFlagState _instance;

    private searchForFlagState()
    {
        if (_instance != null)
        {
            return; //Simply return if there is already an instance of attack
        }

        _instance = this; //set instance to this instance if there isn't already an instance
    }

    public static searchForFlagState Instance
    {
        get
        {
            if (_instance == null)
            {
                new searchForFlagState(); //Create a new instance of attack or return existing
            }

            return _instance;
        }
    }
    //End of singleton implementation

    public override void EnterState(StateMachineAI bot)
    {
        bot.getActions().MoveTo(bot.getEnemyFlagObj());
    }

    public override void ExitState(StateMachineAI bot)
    {

    }

    public override void UpdateState(StateMachineAI bot)
    {
        bot.idleState();
    }
}
