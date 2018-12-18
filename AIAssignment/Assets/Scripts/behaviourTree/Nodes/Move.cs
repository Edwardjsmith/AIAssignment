using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Node
{
    boolAction<Vector3> act;
    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        start = false;
        act += ai.getActions().MoveTo;
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();
        if(getState() != nodeState.Success)
        {
            moveTo(ai.getTargetPosition().position);
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
            act(targetPos);
            start = true;
        }
        if(Vector3.Distance(ai.gameObject.transform.position, targetPos) < 1)
        {
            succeed();
        }
    }


}
