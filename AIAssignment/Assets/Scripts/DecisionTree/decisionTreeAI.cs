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


        friendlyFlagAtBase.addNoChild(seenHealth);

        seenHealth.addYesChild(isHealthInRange);
        seenHealth.addNoChild(seenPowerup);

        isHealthInRange.addYesChild(pickupHealth);
        isHealthInRange.addNoChild(makeMoveToHealth);

        seenPowerup.addYesChild(ispowerInRange);
        seenPowerup.addNoChild(isEnemyNear);

        ispowerInRange.addYesChild(pickuppowerup);
        ispowerInRange.addNoChild(makeMoveToPowerup);

        isEnemyNear.addYesChild(haveEnemyFlagTwo);
        isEnemyNear.addNoChild(isEnemyFlagAtBase);

        haveEnemyFlagTwo.addYesChild(atBaseEnemyFlag);
        haveEnemyFlagTwo.addNoChild(checkMyHealth);

        checkMyHealth.addYesChild(checkForPowerUp);
        checkMyHealth.addNoChild(checkForHealthKit);

        isEnemyFlagAtBase.addYesChild(makeMoveToDefend);
        isEnemyFlagAtBase.addNoChild(haveEnemyFlag);

        checkForPowerUp.addYesChild(usePowerUp);
        checkForPowerUp.addNoChild(isInAttackRange);

        checkForHealthKit.addYesChild(useHealthKit);
        checkForHealthKit.addNoChild(makeMoveToHealth);

        isInAttackRange.addYesChild(attackEnemy);
        isInAttackRange.addNoChild(makeMoveToEnemy);

        haveEnemyFlag.addYesChild(atBaseEnemyFlag);
        haveEnemyFlag.addNoChild(isEnemyFlagInRange);

        atBaseEnemyFlag.addYesChild(dropEnemyFlag);
        atBaseEnemyFlag.addNoChild(makeMoveToBase);

        isEnemyFlagInRange.addYesChild(pickUpEnemyFlag);
        isEnemyFlagInRange.addNoChild(makeMoveToEnemyFlag);

       

        friendlyFlagAtBase.addYesChild(seenHealthTwo);

        seenHealthTwo.addYesChild(isHealthInRange);
        seenHealthTwo.addNoChild(seenPowerupTwo);

        seenPowerupTwo.addYesChild(ispowerInRange);
        seenPowerupTwo.addNoChild(isEnemyNearTwo);

        isEnemyNearTwo.addYesChild(haveFriendlyFlagTwo);
        isEnemyNearTwo.addNoChild(haveFriendlyFlag);

        haveFriendlyFlagTwo.addYesChild(atBaseFriendlyFlag);
        haveFriendlyFlagTwo.addNoChild(checkMyHealth);

        haveFriendlyFlag.addYesChild(atBaseFriendlyFlag);
        haveFriendlyFlag.addNoChild(friendlyFlagInRange);

        atBaseFriendlyFlag.addYesChild(dropFriendlyFlag);
        atBaseFriendlyFlag.addNoChild(makeMoveToBase);

        friendlyFlagInRange.addYesChild(pickUpFriendlyFlag);
        friendlyFlagInRange.addNoChild(makeMoveToFriendlyFlag);

        
        
        decisionTree = new DecisionTree(friendlyFlagAtBase);

    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();
        decisionTree.executeAction();
	}
}
