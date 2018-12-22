using UnityEngine;
using StateMachine;
public class fleeState: State<AI>
{
    //Make the attack state a singleton to ensure there is only ever one at any time
    private static fleeState _instance;

    private fleeState()
    {
        if (_instance != null)
        {
            return; //Simply return if there is already an instance of attack
        }

        _instance = this; //set instance to this instance if there isn't already an instance
    }

    public static fleeState Instance
    {
        get
        {
            if (_instance == null)
            {
                new fleeState(); //Create a new instance of attack or return existing
            }

            return _instance;
        }
    }
    //End of singleton implementation

    public override void EnterState(AI bot)
    {
        bot.getActions().Flee(bot.getTargetObj());
    }

    public override void ExitState(AI bot)
    {

    }

    public override void UpdateState(AI bot)
    {
        bot.flee();
    }
}
