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
        public bool isAction;  

        public Node(decisionTreeAI bot)
        {
            AI = bot;
        }

        public virtual Node getBranch()
        {
            return null;
        }
   
        public virtual void executeAction() { }
    }

    
    class decisionNode : Node
    {
        protected List<Node> children;
        protected Node chosenBranch;

        delegates.decision decision;

        public decisionNode(decisionTreeAI bot, delegates.decision decision) : base(bot)
        {
            isAction = false;
            children = new List<Node>();
            this.decision = decision;
        }

        public void addChildren(Node childLeft, Node childRight)
        {
            children.Insert(0, childLeft);
            children.Insert(1, childRight);
        }

        public override Node getBranch()
        {
            evaluateChildren();

            if (chosenBranch.isAction)
            {
                chosenBranch.executeAction();
                return chosenBranch;
            }
            else
            {
                return chosenBranch;
            }
        }

        protected virtual void evaluateChildren()
        {
            chosenBranch = decision.Invoke(AI) ? children[0] : children[1];
        }
    }

    class gameObjectDecisonNode : decisionNode
    {
        delegates.gameObjectDecision decision;
        GameObject obj;

        public gameObjectDecisonNode(decisionTreeAI bot, delegates.gameObjectDecision decision, GameObject obj) : base(bot, null)
        {
            this.decision = decision;
            this.obj = obj;
        }

        protected override void evaluateChildren()
        {
            chosenBranch = decision.Invoke(AI, obj) ? children[0] : children[1];
        }
    }

    class vector3DecisionNode : decisionNode
    {
        delegates.vector3Decision decision;
        Vector3 position;

        public vector3DecisionNode(decisionTreeAI bot, delegates.vector3Decision decision, Vector3 Pos) : base(bot, null)
        {
            this.decision = decision;
            position = Pos;
        }

        protected override void evaluateChildren()
        {
            chosenBranch = decision.Invoke(AI, position) ? children[0] : children[1];
        }
    }

    class actionNode : Node
    {
        delegates.action action;

        public actionNode(decisionTreeAI bot, delegates.action action) : base(bot)
        {
            base.isAction = true;
            this.action = action;
        }

        public override void executeAction()
        {
            action.Invoke(AI);
        }
    }

    class gameObjectActionNode : actionNode
    {
        delegates.gameObjectAction action;
        GameObject obj;

        public gameObjectActionNode(decisionTreeAI bot, delegates.gameObjectAction action, GameObject obj) : base(bot, null)
        {
            this.action = action;
            this.obj = obj;
        }

        public override void executeAction()
        {
            action.Invoke(AI, obj);
        }
    }

    class vector3ActionNode : actionNode
    {
        delegates.vector3Action action;
        Vector3 position;

        public vector3ActionNode(decisionTreeAI bot, delegates.vector3Action action, Vector3 Pos) : base(bot, null)
        {
            this.action = action;
            position = Pos;
        }

        public override void executeAction()
        {
            action.Invoke(AI, position);
        }
    }

    class DecisionTree
    {
        Node rootNode;
        
        public DecisionTree(Node root)
        {
            rootNode = root;
        }

        void makeDecision(Node current)
        {
            if (current != null)
            {
                makeDecision(current.getBranch());
            }
        }

        public void updateTree()
        {
            makeDecision(rootNode);
        }
    }
}
