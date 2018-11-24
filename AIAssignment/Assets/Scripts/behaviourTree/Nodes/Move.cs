using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Node
{
    bool start;

    action<Vector3> act;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
        start = false;
        act += ai._agentActions.MoveTo;
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();
        if(getState() != nodeState.Success)
        {
            act(ai.targetPos);
        }
	}

    public override void reset()
    {
        Start();
    }

    public void moveTo(Vector3 targetPos)
    {
        if (!start)
        {
            ai._agentActions.MoveTo(targetPos);
            start = true;
        }
        if(ai.gameObject.transform.position == targetPos)
        {
            succeed();
        }
    }


}
