using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Decisiontree
{
    struct delegates
    {
        public delegate bool decision (decisionTreeAI bot);
        public delegate bool gameObjectDecision(decisionTreeAI bot, GameObject obj);

        public delegate bool action(decisionTreeAI bot);
        public delegate bool gameObjectAction(decisionTreeAI bot, GameObject obj);

    }
   class Actions
    {
        public static bool Move(decisionTreeAI bot, GameObject target)
        {
            Debug.Log(target);
            return bot.getActions().MoveTo(target); 
        }

        public static bool Attack(decisionTreeAI bot)
        {
            bot.getActions().AttackEnemy(bot.getTargetObj());
            return true;
        }

        public static bool dropItem(decisionTreeAI bot, GameObject item)
        {
            if(bot.GetInventory().HasItem(item.name))
            {
                bot.getActions().DropItem(item);
                if(item == bot.getEnemyFlagObj())
                {
                    bot.setFlagCaptured();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool pickUpItem(decisionTreeAI bot, GameObject item)
        {
            bot.getActions().CollectItem(item);
            return true;
        }

        public static bool useItem(decisionTreeAI bot, GameObject item)
        {
            bot.getActions().UseItem(item);
            return true;
        }
    }

    class Decisions
    {
        public static bool isEnemyFlagCaptured(decisionTreeAI bot)
        {
            return bot.enemyFlagAtBase();
        }
        public static bool ItemInRange(decisionTreeAI bot, GameObject item)
        {
            if (Vector3.Distance(bot.transform.position, item.transform.position) <= bot.getData().PickUpRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool checkHealth(decisionTreeAI bot)
        {
            if (bot.getData().CurrentHitPoints > bot.getData().CurrentHitPoints / 2)
            {
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool haveItem(decisionTreeAI bot, GameObject item)
        {
            if(bot.GetInventory().HasItem(item.name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool inAttackRange(decisionTreeAI bot)
        {
            if (Vector3.Distance(bot.transform.position, bot.nearestEnemy.transform.position) <= bot.getData().AttackRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool enemySeen(decisionTreeAI bot)
        {
            if (bot.getSenses().GetEnemiesInView() != null)
            {
                bot.setTarget(bot.nearestEnemy);

                if (bot.getTargetObj() != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool atBase(decisionTreeAI bot)
        {
            if(Vector3.Distance(bot.transform.position, bot.getBase().transform.position) < 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class Node
    {
        protected decisionTreeAI AI;
        protected bool _isLeaf;

        public bool isLeaf()
        {
           return _isLeaf; 
        }

        public Node(decisionTreeAI bot)
        {
            AI = bot;
        }

        public virtual Node makeDecision()
        {
            return null;
        }
   
        public virtual void executeAction() { }
    }

    
    class decisionNode : Node
    {
        protected Node yes;
        protected Node no;

        delegates.decision _decision;

        public decisionNode(decisionTreeAI bot, delegates.decision decision) : base(bot)
        {
            _isLeaf = false;
            yes = null;
            no = null;
            _decision = decision;
        }

        public void addYesChild(Node child)
        {
            yes = child;
        }

        public void addNoChild(Node child)
        {
            no = child;
        }

        public override Node makeDecision()
        {
            if (_decision.Invoke(AI))
            {
                return yes;
            }
            else
            {
                return no;
            }
        }
    }

    class gameObjectDecisonNode : decisionNode
    {
        delegates.gameObjectDecision _decision;
        GameObject _obj;

        public gameObjectDecisonNode(decisionTreeAI bot, delegates.gameObjectDecision decision, GameObject obj) : base(bot, null)
        {
            _decision = decision;
            _obj = obj;
        }

        public override Node makeDecision()
        {
            if (_decision.Invoke(AI, _obj))
            {
                return yes;
            }
            else
            {
                return no;
            }
        }
    }

    class actionNode : Node
    {
        delegates.action _action;

        public actionNode(decisionTreeAI bot, delegates.action action) : base(bot)
        {
            _isLeaf = true;
            _action = action;
        }

        public override void executeAction()
        {
            _action.Invoke(AI);
        }
    }

    class gameObjectActionNode : actionNode
    {
        delegates.gameObjectAction _action;
        GameObject _obj;

        public gameObjectActionNode(decisionTreeAI bot, delegates.gameObjectAction action, GameObject obj) : base(bot, null)
        {
            _action = action;
            _obj = obj;
        }

        public override void executeAction()
        {
            _action.Invoke(AI, _obj);
        }
    }

    class DecisionTree
    {
        Node rootNode;
        Node currentNode;
        
        public DecisionTree(Node root)
        {
            rootNode = root;
            currentNode = null;
        }

        void TraverseTree(Node current)
        {
            if (current.isLeaf())
            {
                current.executeAction();
            }
            else
            {
                current = current.makeDecision();
                TraverseTree(current);
            }
        }

        public void executeAction()
        {
            TraverseTree(rootNode);
        }
    }
}
