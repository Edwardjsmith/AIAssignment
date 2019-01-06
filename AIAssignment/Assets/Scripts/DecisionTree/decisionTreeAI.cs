using Decisiontree;
using UnityEngine;
using treeAttributes;
public class decisionTreeAI : AI {

    DecisionTree decisionTree;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();

        delegates.decision isEnemyFlagAtFriendlyBase = new delegates.decision(Decisions.isEnemyFlagCaptured);
        delegates.decision enemyNear = new delegates.decision(Decisions.enemySeen);
        delegates.gameObjectDecision itemSeen = new delegates.gameObjectDecision(Decisions.itemSeen);
        delegates.gameObjectDecision checkInv = new delegates.gameObjectDecision(Decisions.haveItem);
        delegates.decision atbase = new delegates.decision(Decisions.atBase);
        delegates.gameObjectDecision itemWithinRange = new delegates.gameObjectDecision(Decisions.ItemInRange);
        delegates.decision inAttackRange = new delegates.decision(Decisions.inAttackRange);
        delegates.decision checkhealth = new delegates.decision(Decisions.checkHealth);
        delegates.decision checkifPoweredUp = new delegates.decision(Decisions.isPoweredUp);
        delegates.decision checkFriendlyFlag = new delegates.decision(Decisions.friendlyFlagTaken);

        delegates.gameObjectAction moveToGameObj = new delegates.gameObjectAction(Actions.MoveToGameObj);
        delegates.vector3Action moveToVec3 = new delegates.vector3Action(Actions.MoveToVec3);
        delegates.action moveToEnemy = new delegates.action(Actions.MoveToEnemy);
        delegates.action attack = new delegates.action(Actions.Attack);
        delegates.gameObjectAction dropflag = new delegates.gameObjectAction(Actions.dropItem);
        delegates.gameObjectAction pickUpItem = new delegates.gameObjectAction(Actions.pickUpItem);
        delegates.gameObjectAction useitem = new delegates.gameObjectAction(Actions.useItem);

        decisionNode friendlyFlagAtBase = new decisionNode(this, checkFriendlyFlag);
        decisionNode isEnemyFlagAtBase = new decisionNode(this, isEnemyFlagAtFriendlyBase);
        decisionNode isEnemyNear = new decisionNode(this, enemyNear);
        decisionNode isEnemyNearTwo = new decisionNode(this, enemyNear);

        gameObjectDecisonNode haveEnemyFlag = new gameObjectDecisonNode(this, checkInv, getEnemyFlagObj());
        gameObjectDecisonNode checkForHealthKit = new gameObjectDecisonNode(this, checkInv, getHealthKit());
        gameObjectDecisonNode checkForPowerUp = new gameObjectDecisonNode(this, checkInv, getPowerUp());
        decisionNode poweredUp = new decisionNode(this, checkifPoweredUp);
        decisionNode atBaseFriendlyFlag = new decisionNode(this, atbase);
        decisionNode atBaseEnemyFlag = new decisionNode(this, atbase);
        gameObjectDecisonNode isEnemyFlagInRange = new gameObjectDecisonNode(this, itemWithinRange, getEnemyFlagObj());
        gameObjectDecisonNode isHealthInRange = new gameObjectDecisonNode(this, itemWithinRange, getHealthKit());
        decisionNode isInAttackRange = new decisionNode(this, inAttackRange);
        decisionNode checkMyHealth = new decisionNode(this, checkhealth);
        gameObjectDecisonNode ispowerInRange = new gameObjectDecisonNode(this, itemWithinRange, getPowerUp());
        gameObjectDecisonNode haveFriendlyFlag = new gameObjectDecisonNode(this, checkInv, getFriendlyFlagObj());
        gameObjectDecisonNode friendlyFlagInRange = new gameObjectDecisonNode(this, itemWithinRange, getFriendlyFlagObj());

        gameObjectDecisonNode seenHealth = new gameObjectDecisonNode(this, itemSeen, getHealthKit());
        gameObjectDecisonNode seenHealthTwo = new gameObjectDecisonNode(this, itemSeen, getHealthKit());
        gameObjectDecisonNode seenPowerup = new gameObjectDecisonNode(this, itemSeen, getPowerUp());
        gameObjectDecisonNode seenPowerupTwo = new gameObjectDecisonNode(this, itemSeen, getPowerUp());

        gameObjectActionNode makeMoveToEnemyFlag = new gameObjectActionNode(this, moveToGameObj, getEnemyFlagObj());
        gameObjectActionNode makeMoveToFriendlyFlag = new gameObjectActionNode(this, moveToGameObj, getFriendlyFlagObj());
        gameObjectActionNode makeMoveToBase = new gameObjectActionNode(this, moveToGameObj, Base);
        vector3ActionNode makeMoveToDefend = new vector3ActionNode(this, moveToVec3, defencePoint);
        actionNode makeMoveToEnemy = new actionNode(this, moveToEnemy);

        gameObjectActionNode makeMoveToHealth = new gameObjectActionNode(this, moveToGameObj, getHealthKit());
        gameObjectActionNode makeMoveToPowerup = new gameObjectActionNode(this, moveToGameObj, getPowerUp());

        actionNode attackEnemy = new actionNode(this, attack);
        gameObjectActionNode dropEnemyFlag = new gameObjectActionNode(this, dropflag, getEnemyFlagObj());
        gameObjectActionNode dropFriendlyFlag = new gameObjectActionNode(this, dropflag, getFriendlyFlagObj());
        gameObjectActionNode pickUpEnemyFlag = new gameObjectActionNode(this, pickUpItem, getEnemyFlagObj());
        gameObjectActionNode pickUpFriendlyFlag = new gameObjectActionNode(this, pickUpItem, getFriendlyFlagObj());
        gameObjectActionNode pickupHealth = new gameObjectActionNode(this, pickUpItem, getHealthKit());
        gameObjectActionNode useHealthKit = new gameObjectActionNode(this, useitem, getHealthKit());
        gameObjectActionNode usePowerUp = new gameObjectActionNode(this, useitem, getPowerUp());
        gameObjectActionNode pickuppowerup = new gameObjectActionNode(this, pickUpItem, getPowerUp());

        gameObjectDecisonNode haveFriendlyFlagTwo = new gameObjectDecisonNode(this, checkInv, getFriendlyFlagObj());
        gameObjectDecisonNode haveEnemyFlagTwo = new gameObjectDecisonNode(this, checkInv, getEnemyFlagObj());


        friendlyFlagAtBase.addLeftChild(seenHealth);
        friendlyFlagAtBase.addRightChild(seenHealthTwo);

        seenHealthTwo.addLeftChild(isHealthInRange);
        seenHealthTwo.addRightChild(seenPowerupTwo);

        seenPowerupTwo.addLeftChild(ispowerInRange);
        seenPowerupTwo.addRightChild(isEnemyNearTwo);

        isEnemyNearTwo.addLeftChild(haveFriendlyFlagTwo);
        isEnemyNearTwo.addRightChild(haveFriendlyFlag);

        haveFriendlyFlagTwo.addLeftChild(atBaseFriendlyFlag);
        haveFriendlyFlagTwo.addRightChild(checkMyHealth);

        haveFriendlyFlag.addLeftChild(atBaseFriendlyFlag);
        haveFriendlyFlag.addRightChild(friendlyFlagInRange);

        atBaseFriendlyFlag.addLeftChild(dropFriendlyFlag);
        atBaseFriendlyFlag.addRightChild(makeMoveToBase);

        friendlyFlagInRange.addLeftChild(pickUpFriendlyFlag);
        friendlyFlagInRange.addRightChild(makeMoveToFriendlyFlag);

        seenHealth.addLeftChild(isHealthInRange);
        seenHealth.addRightChild(seenPowerup);

        isHealthInRange.addLeftChild(pickupHealth);
        isHealthInRange.addRightChild(makeMoveToHealth);

        seenPowerup.addLeftChild(ispowerInRange);
        seenPowerup.addRightChild(isEnemyNear);

        ispowerInRange.addLeftChild(pickuppowerup);
        ispowerInRange.addRightChild(makeMoveToPowerup);

        isEnemyNear.addLeftChild(haveEnemyFlagTwo);
        isEnemyNear.addRightChild(isEnemyFlagAtBase);

        haveEnemyFlagTwo.addLeftChild(atBaseEnemyFlag);
        haveEnemyFlagTwo.addRightChild(checkMyHealth);

        checkMyHealth.addLeftChild(checkForPowerUp);
        checkMyHealth.addRightChild(checkForHealthKit);

        isEnemyFlagAtBase.addLeftChild(makeMoveToDefend);
        isEnemyFlagAtBase.addRightChild(haveEnemyFlag);

        checkForPowerUp.addLeftChild(usePowerUp);
        checkForPowerUp.addRightChild(isInAttackRange);

        checkForHealthKit.addLeftChild(useHealthKit);
        checkForHealthKit.addRightChild(makeMoveToHealth);

        isInAttackRange.addLeftChild(attackEnemy);
        isInAttackRange.addRightChild(makeMoveToEnemy);

        haveEnemyFlag.addLeftChild(atBaseEnemyFlag);
        haveEnemyFlag.addRightChild(isEnemyFlagInRange);

        atBaseEnemyFlag.addLeftChild(dropEnemyFlag);
        atBaseEnemyFlag.addRightChild(makeMoveToBase);

        isEnemyFlagInRange.addLeftChild(pickUpEnemyFlag);
        isEnemyFlagInRange.addRightChild(makeMoveToEnemyFlag);




        decisionTree = new DecisionTree(friendlyFlagAtBase);

    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();
        decisionTree.executeAction();
        //Debug.Log(decisionTree.returnCurrent().nodeName);
	}
}
