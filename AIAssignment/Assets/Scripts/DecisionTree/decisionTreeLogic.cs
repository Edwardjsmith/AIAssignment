using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DecisionTree
{

    struct delegates
    {
        public delegate bool decision(AI bot);
        public delegate bool action(AI bot);
    }
   class Actions
    {
        public static bool Move(AI bot)
        {
            return bot.getActions().MoveTo(bot.getEnemyFlagObj());
        }
    }

    class Decisions
    {

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

        decisionNode(decisionTreeAI bot, delegates.decision decision) : base(bot)
        {
            _isLeaf = false;
            yes = null;
            no = null;
            _decision = decision;
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

        actionNode(decisionTreeAI bot, delegates.action Action) : base(bot)
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
