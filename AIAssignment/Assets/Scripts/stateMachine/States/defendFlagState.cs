
using UnityEngine;
using StateMachine;

public class defendFlagState : State<AI>
{
    //Make the attack state a singleton to ensure there is only ever one at any time
    private static defendFlagState _instance;

    private defendFlagState()
    {
        if (_instance != null)
        {
            return; //Simply return if there is already an instance of attack
        }

        _instance = this; //set instance to this instance if there isn't already an instance
    }

    public static defendFlagState Instance
    {
        get
        {
            if (_instance == null)
            {
                new defendFlagState(); //Create a new instance of attack or return existing
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
        bot.defendFlag();
    }
}
