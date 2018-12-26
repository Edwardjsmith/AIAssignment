using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DecisionTree;

public class decisionTreeAI : AI {

	// Use this for initialization
	public override void Start ()
    {
        delegates.action moveToFlag = new delegates.action(Actions.Move);

	}
	
	// Update is called once per frame
	public override void Update () {
		
	}
}
