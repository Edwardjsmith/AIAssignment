using Decisiontree;
using UnityEngine;

public class decisionTreeAI : AI {

    DecisionTree decisionTree;

    public GameObject nearestEnemy;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();

        delegates.decision isEnemyFlagAtFriendlyBase = new delegates.decision(Decisions.isEnemyFlagCaptured);
        delegates.decision enemyNear = new delegates.decision(Decisions.enemySeen);
        delegates.gameObjectDecision CheckForEnemyFlag = new delegates.gameObjectDecision(Decisions.haveItem);
        delegates.decision atbase = new delegates.decision(Decisions.atBase);
        delegates.gameObjectDecision itemWithinRange = new delegates.gameObjectDecision(Decisions.ItemInRange);
        delegates.decision inAttackRange = new delegates.decision(Decisions.inAttackRange);
        delegates.decision checkhealth = new delegates.decision(Decisions.checkHealth);


        delegates.gameObjectAction moveTo = new delegates.gameObjectAction(Actions.Move);
        delegates.action attack = new delegates.action(Actions.Attack);
        delegates.gameObjectAction dropflag = new delegates.gameObjectAction(Actions.dropItem);
        delegates.gameObjectAction pickupflag = new delegates.gameObjectAction(Actions.pickUpItem);
        delegates.gameObjectAction useitem = new delegates.gameObjectAction(Actions.useItem);

        decisionNode isEnemyFlagAtBase = new decisionNode(this, isEnemyFlagAtFriendlyBase);
        decisionNode isEnemyNear = new decisionNode(this, enemyNear);
        gameObjectDecisonNode checkEnemyFlag = new gameObjectDecisonNode(this, CheckForEnemyFlag, getEnemyFlagObj());
        decisionNode amIAtBase = new decisionNode(this, atbase);
        gameObjectDecisonNode isFlagInRange = new gameObjectDecisonNode(this, itemWithinRange, getEnemyFlagObj());
        decisionNode isInAttackRange = new decisionNode(this, inAttackRange);
        decisionNode checkMyHealth = new decisionNode(this, checkhealth);

        gameObjectActionNode makeMoveToFlag = new gameObjectActionNode(this, moveTo, getEnemyFlagObj());
        gameObjectActionNode makeMoveToBase = new gameObjectActionNode(this, moveTo, Base);
        gameObjectActionNode makeMoveToDefend = new gameObjectActionNode(this, moveTo, defenceObject);
        gameObjectActionNode makeMoveToEnemy = new gameObjectActionNode(this, moveTo, nearestEnemy);

        actionNode attackEnemy = new actionNode(this, attack);
        gameObjectActionNode dropThisFlag = new gameObjectActionNode(this, dropflag, getEnemyFlagObj());
        gameObjectActionNode pickupthisflag = new gameObjectActionNode(this, pickupflag, getEnemyFlagObj());
        gameObjectActionNode useThisitem = new gameObjectActionNode(this, useitem, getTargetObj());

        /*isEnemyNear.addYesChild(isInAttackRange);
        isEnemyNear.addNoChild(checkFlag);

        isInAttackRange.addYesChild(attackEnemy);
        isInAttackRange.addNoChild(checkMyHealth);

        checkMyHealth.addYesChild(makeMove);
        //Need to implement health kit search here

        checkFlag.addYesChild(isAtBase);
        checkFlag.addNoChild(isFlagInRange);

        isAtBase.addYesChild(dropThisFlag);
        isAtBase.addNoChild(makeMove);

        isFlagInRange.addYesChild(pickupthisflag);
        isFlagInRange.addNoChild(makeMove);*/

        isEnemyNear.addNoChild(isEnemyFlagAtBase);
        isEnemyNear.addYesChild(isInAttackRange);

        isInAttackRange.addYesChild(attackEnemy);
        isInAttackRange.addNoChild(makeMoveToEnemy);

        isEnemyFlagAtBase.addNoChild(checkEnemyFlag);
        isEnemyFlagAtBase.addYesChild(makeMoveToDefend);

        checkEnemyFlag.addYesChild(amIAtBase);
        amIAtBase.addYesChild(dropThisFlag);
        amIAtBase.addNoChild(makeMoveToBase);

        checkEnemyFlag.addNoChild(isFlagInRange);
        isFlagInRange.addYesChild(pickupthisflag);
        isFlagInRange.addNoChild(makeMoveToFlag);

        decisionTree = new DecisionTree(isEnemyNear);
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        if(getSenses().GetEnemiesInView() != null)
        {
            nearestEnemy = GetClosestObject(getSenses().GetEnemiesInView());
        }

        decisionTree.executeAction();
	}
}
