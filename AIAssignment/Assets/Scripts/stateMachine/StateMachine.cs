
namespace StateMachine
{
    public class stateMachine<StateMachineAI> //Could easily be set to take a type T for extra flexibility but since all of my scripts will derive from AI, I can be explicit
    {
        public State<StateMachineAI> currentState { get; set; } //Used to store, check and set current state
        public StateMachineAI ai; 

        public stateMachine(StateMachineAI bot)
        {
            ai = bot; //reference the AI that this statemachine applies to
            currentState = null; //set initial state to null ready to be set again later
        }

        public void Update()
        {
            if(currentState != null)
            {
                currentState.UpdateState(ai); //As long as we are in a state, update..
            }
        }

        public void transitionToNewState(State<StateMachineAI> nextState)
        {
            if(currentState != null)
            {
                currentState.ExitState(ai); //If we are currently in a state, run exit code for that state
            }

            //assign new state to current state and run entry code for that state
            currentState = nextState;
            currentState.EnterState(ai);
        }
    }

    public abstract class State<StateMachineAI>
    {
        public abstract void EnterState(StateMachineAI bot);
        public abstract void ExitState(StateMachineAI bot);
        public abstract void UpdateState(StateMachineAI bot);
    }
}
