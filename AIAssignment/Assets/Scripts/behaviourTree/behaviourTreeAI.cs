﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class behaviourTreeAI : AI
{
    public GameObject enemy;

    Node move;
    Node attackEnemy;
    Node fleeEnemy;
    Node collectItem;
    Node useItem;
    Node dropItem;
    Node dropAllItems;

    Sequence moveAndAttack;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();

        target = enemy;
        targetPos = target.transform;

        move = new Move();
        attackEnemy = new Attack();

        moveAndAttack = new Sequence();
        moveAndAttack.initialiseNode(this);
        moveAndAttack.Start();
        moveAndAttack.addAction(move);
        moveAndAttack.addAction(attackEnemy);

        moveAndAttack.initialiseSequence();

        
	}
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();
        moveAndAttack.Update();
	}
}
