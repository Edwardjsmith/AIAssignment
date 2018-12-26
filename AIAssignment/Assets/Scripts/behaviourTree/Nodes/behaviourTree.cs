using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Node
    {
        public enum nodeState { Success, Failure, Running };

        protected nodeState state;
        protected AI ai;

        protected bool start;

        public void initialiseNode(AI bot)
        {
            ai = bot;
        }

        // Use this for initialization
        public virtual void Start()
        {
            state = nodeState.Running;
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (getState() == nodeState.Running)
            {

            }
        }

        public delegate bool boolAction<T>(T type);
        public delegate void voidAction<T>(T type);

        public abstract void reset();

        protected void succeed()
        {
            state = nodeState.Success;
        }

        protected void fail()
        {
            state = nodeState.Failure;
        }

        public nodeState getState()
        {
            return state;
        }
    }

    public class Move : Node
    {
        boolAction<Vector3> act;
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            start = false;
            act += ai.getActions().MoveTo;
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            if (getState() != nodeState.Success)
            {
                moveTo(ai.getTargetPosition());
            }
        }

        public override void reset()
        {
            Start();
        }

        public void moveTo(Vector3 targetPos)
        {
            if (!start)
            {
                act(targetPos);
                start = true;
            }
            if (Vector3.Distance(ai.gameObject.transform.position, targetPos) < 1)
            {
                succeed();
            }
        }

    }

    public class Attack : Node
    {

        voidAction<GameObject> act;
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            start = false;
            act += ai.getActions().AttackEnemy;
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            if (getState() != nodeState.Success)
            {
                attack(ai.getTargetObj());
                Debug.Log("Attack successful");
            }
        }

        public override void reset()
        {
            Start();
        }

        public void attack(GameObject target)
        {
            if (!start)
            {
                act(target);
                start = true;

                if (ai.getSenses().IsInAttackRange(ai.getTargetObj()))
                {
                    succeed();
                }
                else
                {
                    fail();
                }
            }
        }
    }

    public class enemyNear : Node
    {
        public override void reset()
        {
            Start();
        }

        // Use this for initialization
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        public void checkForEnemy()
        {

        }
    }

    public class Sequence : Node
    {

        List<Node> nodes;
        Queue<Node> sequence;

        private Node currentAction;

        // Use this for initialization
        public override void Start()
        {
            nodes = new List<Node>();
            sequence = new Queue<Node>();
        }

        // Update is called once per frame
        public override void Update()
        {
            currentAction.Update();

            if (currentAction.getState() == nodeState.Success)
            {
                succeed();
                if (sequence.Peek() == null)
                {
                    state = currentAction.getState();
                }
                else
                {
                    currentAction = sequence.Peek();
                    currentAction.initialiseNode(ai);
                    currentAction.Start();
                }
            }
        }
        public void addAction(Node action)
        {
            nodes.Add(action);
        }
        public void initialiseSequence()
        {
            sequence.Clear();
            foreach (Node node in nodes)
            {
                sequence.Enqueue(node);
            }

            currentAction = sequence.Peek();
            currentAction.initialiseNode(ai);
            currentAction.Start();
        }
        public override void reset()
        {
            foreach (Node node in nodes)
            {
                node.reset();
            }
        }
    }



    public class behaviourTree
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
