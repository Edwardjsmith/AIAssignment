using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node {

    List<Node> nodes;
    Queue<Node> sequence;

    private Node currentAction;

	// Use this for initialization
	new void Start ()
    {
        nodes = new List<Node>();
        sequence = new Queue<Node>();
	}
	
	// Update is called once per frame
	new void Update ()
    {
        currentAction.Update();

        if (currentAction.getState() == nodeState.Success)
        {
            succeed();

            if (sequence.Peek() == null)
            {
                state = currentAction.getState();
            }
            else
            {
                currentAction = sequence.Peek();
                currentAction.initialiseNode(ai);
                currentAction.Start();
            }
        }
	}
    public void addAction(Node action)
    {
        nodes.Add(action);
    }
    public void initialiseSequence()
    {
        sequence.Clear();
        foreach (Node node in nodes)
        {
            sequence.Enqueue(node);
        }

        currentAction = sequence.Peek();
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
