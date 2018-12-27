

using UnityEngine;
using StateMachine;

public class pickupItemState : State<StateMachineAI>
{

    //Make the attack state a singleton to ensure there is only ever one at any time
    private static pickupItemState _instance;

    private pickupItemState()
    {
        if (_instance != null)
        {
            return; //Simply return if there is already an instance of attack
        }

        _instance = this; //set instance to this instance if there isn't already an instance
    }

    public static pickupItemState Instance
    {
        get
        {
            if (_instance == null)
            {
                new pickupItemState(); //Create a new instance of attack or return existing
            }

            return _instance;
        }
    }
    //End of singleton implementation

    public override void EnterState(StateMachineAI bot)
    {
        bot.setTarget(bot.getPowerUp());
        bot.getActions().MoveTo(bot.getTargetPosition());
    }

    public override void ExitState(StateMachineAI bot)
    {

    }

    public override void UpdateState(StateMachineAI bot)
    {
        bot.pickupItem();
    }
}
