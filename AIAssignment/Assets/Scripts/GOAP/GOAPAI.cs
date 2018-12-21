using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct actions
{
    public delegate bool boolAction<T>(T type);
    boolAction<Vector3> boolAct;
    public delegate void voidAction<T>(T type);
    voidAction<GameObject> voidAct;

    float cost;
}

public class GOAPAI : AI
{
    actions move;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
