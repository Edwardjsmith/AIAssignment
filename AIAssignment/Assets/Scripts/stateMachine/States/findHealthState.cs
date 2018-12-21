using UnityEngine;
using StateMachine;

public class findHealthState : State<AI>
{
    //Make the attack state a singleton to ensure there is only ever one at any time
    private static findHealthState _instance;

    private findHealthState()
    {
        if (_instance != null)
        {
            return; //Simply return if there is already an instance of attack
        }

        _instance = this; //set instance to this instance if there isn't already an instance
    }

    public static findHealthState Instance
    {
        get
        {
            if (_instance == null)
            {
                new findHealthState(); //Create a new instance of attack or return existing
            }

            return _instance;
        }
    }
    //End of singleton implementation

    public override void EnterState(AI bot)
    {

    }

    public override void ExitState(AI bot)
    {

    }

    public override void UpdateState(AI bot)
    {
        bot.findHealth();
    }
}
