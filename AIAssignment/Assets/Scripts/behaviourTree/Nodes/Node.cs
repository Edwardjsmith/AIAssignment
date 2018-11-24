using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node{

    public enum nodeState { Success, Failure, Running };

    protected nodeState state;
    protected AI ai;

    public void initialiseNode(AI bot)
    {
        ai = bot;
    }

    // Use this for initialization
    public virtual void Start ()
    {
        state = nodeState.Running;
	}
	
	// Update is called once per frame
	public virtual void Update ()
    {
		if(getState() == nodeState.Running)
        {
            
        }
	}

    public delegate bool action<T>(T type);

    public abstract void reset();

    protected void succeed()
    {
        state = nodeState.Success;
    }

    protected void fail()
    {
        state = nodeState.Failure;
    }

   public nodeState getState()
    {
        return state;
    }


}
