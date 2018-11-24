using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node {

    List<Node> nodes;
    Queue<Node> Selection;

    private Node currentAction;

	// Use this for initialization
	new void Start ()
    {
        nodes = new List<Node>();
        Selection = new Queue<Node>();
	}
	
	// Update is called once per frame
	new void Update ()
    {
        currentAction.Update();

        if(currentAction.getState() == nodeState.Success)
        {
            succeed();
        }

        if(Selection.Peek() == null)
        {
            state = currentAction.getState();
        }
        else
        {
            currentAction = Selection.Peek();
            currentAction.initialiseNode(ai);
            currentAction.Start();
        }
	}
    public void addAction(Node action)
    {
        nodes.Add(action);
    }
    public void initialiseSequence()
    {
        Selection.Clear();
        foreach (Node node in nodes)
        {
            Selection.Enqueue(node);
        }

        currentAction = Selection.Peek();
        currentAction.initialiseNode(ai);
        currentAction.Start();
    }
    public override void reset()
    {
        foreach(Node node in nodes)
        {
            node.reset();
        }
    }
}
