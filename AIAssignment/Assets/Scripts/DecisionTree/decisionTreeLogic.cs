using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Decisiontree
{

    struct delegates
    {
        public delegate bool decision (AI bot);
        public delegate bool action(AI bot);
    }
   class Actions
    {
        public static bool Move(AI bot)
        {
            return bot.getActions().MoveTo(bot.getTargetObj());
        }

        public static bool Attack(AI bot)
        {
            bot.getActions().AttackEnemy(bot.getTargetObj());
            return true;
        }

        public static bool dropFlag(AI bot)
        {
            if(bot.GetInventory().HasItem(bot.getEnemyFlagObj().name))
            {
                bot.getActions().DropAllItems();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool pickUpFlag(AI bot)
        {
            bot.getActions().CollectItem(bot.getEnemyFlagObj());
            return true;
        }

        public static bool useItem(AI bot)
        {
            bot.getActions().UseItem(bot.getTargetObj());
            return true;
        }
    }

    class Decisions
    {
        public static bool ItemInRange(AI bot)
        {
            if (Vector3.Distance(bot.transform.position, bot.getTargetObj().transform.position) <= bot.getData().PickUpRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool checkHealth(AI bot)
        {
            if (bot.getData().CurrentHitPoints > bot.getData().CurrentHitPoints / 2)
            {
                return true;
            }
            else
            {
                bot.setTarget(GameObject.Find("Health Kit"));
                return false;
            }
        }

        public static bool haveItem(AI bot)
        {
            if(bot.GetInventory().HasItem(bot.getTargetObj().name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool inAttackRange(AI bot)
        {
            if (Vector3.Distance(bot.transform.position, bot.getTargetObj().transform.position) <= bot.getData().AttackRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool enemySeen(AI bot)
        {
            if (bot.getSenses().GetEnemiesInView() != null)
            {
                bot.setTarget(bot.GetClosestObject(bot.getSenses().GetEnemiesInView()));
                return true;
            }
            else
            {
                bot.setTarget(bot.getEnemyFlagObj());
                return false;
            }
        }

        public static bool checkForItem(AI bot)
        {
            if (bot.GetInventory().HasItem(bot.getTargetObj().name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool atBase(AI bot)
        {
            if(Vector3.Distance(bot.transform.position, bot.getBase().transform.position) > 1)
            {
                bot.setTarget(bot.getBase());
                return false;
            }
            else
            {
                return true;
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
        Node yes;
        Node no;

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

    class actionNode : Node
    {
        delegates.action _action;

        public actionNode(decisionTreeAI bot, delegates.action Action) : base(bot)
        {
            _isLeaf = true;
            _action = Action;
        }

        public override void executeAction()
        {
            _action.Invoke(AI);
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
