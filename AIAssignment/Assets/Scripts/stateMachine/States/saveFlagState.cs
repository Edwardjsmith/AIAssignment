using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
public class saveFlagState : State<StateMachineAI>
{

    //Make the attack state a singleton to ensure there is only ever one at any time
    private static saveFlagState _instance;

    private saveFlagState()
    {
        if (_instance != null)
        {
            return; //Simply return if there is already an instance of attack
        }

        _instance = this; //set instance to this instance if there isn't already an instance
    }

    public static saveFlagState Instance
    {
        get
        {
            if (_instance == null)
            {
                new saveFlagState(); //Create a new instance of attack or return existing
            }

            return _instance;
        }
    }
    //End of singleton implementation

    public override void EnterState(StateMachineAI bot)
    {
        bot.setTarget(bot.getFriendlyFlagObj());
        bot.getActions().MoveTo(bot.getTargetPosition());
    }

    public override void ExitState(StateMachineAI bot)
    {

    }

    public override void UpdateState(StateMachineAI bot)
    {
        bot.saveFlag();
    }
}
