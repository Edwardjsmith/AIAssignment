using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Decisiontree;

public class decisionTreeAI : AI {

    DecisionTree dtTree;
	// Use this for initialization
	public override void Start ()
    {
        base.Start();
        delegates.decision enemyNear = new delegates.decision(Decisions.enemySeen);
        delegates.decision checkForFlag = new delegates.decision(Decisions.checkForItem);
        delegates.decision atbase = new delegates.decision(Decisions.atBase);
        delegates.decision itemWithinRange = new delegates.decision(Decisions.ItemInRange);
        delegates.decision inAttackRange = new delegates.decision(Decisions.inAttackRange);
        delegates.decision checkhealth = new delegates.decision(Decisions.checkHealth);


        delegates.action moveTo = new delegates.action(Actions.Move);
        delegates.action attack = new delegates.action(Actions.Attack);
        delegates.action dropflag = new delegates.action(Actions.dropFlag);
        delegates.action pickupflag = new delegates.action(Actions.pickUpFlag);
        delegates.action useitem = new delegates.action(Actions.useItem);

        decisionNode isEnemyNear = new decisionNode(this, enemyNear);

        decisionNode checkFlag = new decisionNode(this, checkForFlag);
        decisionNode isAtBase = new decisionNode(this, atbase);
        decisionNode isFlagInRange = new decisionNode(this, itemWithinRange);
        decisionNode isInAttackRange = new decisionNode(this, inAttackRange);
        decisionNode checkMyHealth = new decisionNode(this, checkhealth);

        actionNode makeMove = new actionNode(this, moveTo);
        actionNode attackEnemy = new actionNode(this, attack);
        actionNode dropThisFlag = new actionNode(this, dropflag);
        actionNode pickupthisflag = new actionNode(this, pickupflag);
        actionNode useThisitem = new actionNode(this, useitem);

        isFlagInRange.addNoChild(makeMove);
        isFlagInRange.addYesChild(useThisitem);

        isAtBase.addNoChild(makeMove);
        isAtBase.addYesChild(dropThisFlag);

        checkFlag.addNoChild(isFlagInRange);
        checkFlag.addYesChild(isAtBase);

        isFlagInRange.addNoChild(makeMove);
        isFlagInRange.addYesChild(pickupthisflag);

        checkFlag.addNoChild(isFlagInRange);
        checkFlag.addYesChild(useThisitem);

        //Checks for health kit or moves to enemy
        checkMyHealth.addNoChild(checkFlag);
        checkMyHealth.addYesChild(makeMove);

        isInAttackRange.addYesChild(attackEnemy);
        isInAttackRange.addNoChild(checkMyHealth);

        isEnemyNear.addYesChild(isInAttackRange);
        isEnemyNear.addNoChild(checkFlag);

        



        dtTree = new DecisionTree(isEnemyNear);
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        dtTree.executeAction();
	}
}
