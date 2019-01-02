using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using treeAttributes;
namespace Decisiontree
{

    class Node
    {
        protected decisionTreeAI AI;
        protected bool _isLeaf;
        public string nodeName;

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

    class vector3DecisionNode : decisionNode
    {
        delegates.vector3Decision _decision;
        Vector3 Position;

        public vector3DecisionNode(decisionTreeAI bot, delegates.vector3Decision decision, Vector3 Pos) : base(bot, null)
        {
            _decision = decision;
            Position = Pos;
        }

        public override Node makeDecision()
        {
            if(_decision.Invoke(AI, Position))
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
            //nodeName = _action.ToString();
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

    class vector3ActionNode : actionNode
    {
        delegates.vector3Action _action;
        Vector3 Position;

        public vector3ActionNode(decisionTreeAI bot, delegates.vector3Action action, Vector3 Pos) : base(bot, null)
        {
            _action = action;
            Position = Pos;
        }

        public override void executeAction()
        {
            _action.Invoke(AI, Position);
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

            
            currentNode = current;
            if (currentNode.isLeaf())
            {
                current.executeAction();
            }
            else
            {
                currentNode = currentNode.makeDecision();
                TraverseTree(currentNode);
            }
        }

        public Node returnCurrent()
        {
            return currentNode;
        }

        public void executeAction()
        {
            TraverseTree(rootNode);
        }
    }
}
