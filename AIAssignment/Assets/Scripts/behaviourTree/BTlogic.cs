using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using treeAttributes;

namespace BehaviourTree
{ 

   public class task : MonoBehaviour
    {
        public List<task> childTasks;
        protected bool _result = false;
        protected bool isFinished = false;

        protected behaviourTreeAI _bot; 
        public task(behaviourTreeAI bot)
        {
            _bot = bot;
        }

        public virtual void setResult(bool result)
        {
            _result = result;
            isFinished = true;
        }
        public bool getResult()
        {
            return _result;
        }

        public virtual IEnumerator run()
        {
            setResult(true);
            yield break;
        }

        public virtual IEnumerator runTask()
        {
            yield return StartCoroutine(run());
        }
    }

    public class condition : task
    {
        delegates.decision _decision;
        public condition(behaviourTreeAI bot, delegates.decision decision) : base(bot)
        {
            _decision = decision;
        }
        public override IEnumerator run()
        {
            isFinished = false;

            setResult(_decision.Invoke(_bot));
            yield break;
        }

    }

    public class Action : task
    {
        delegates.action _action;
        public Action(behaviourTreeAI bot, delegates.action action) : base(bot)
        {
            _action = action;
        }
        public override IEnumerator run()
        {
            isFinished = false;
            setResult(_action.Invoke(_bot));
            yield break;
        }
    }

  public class selector : task
    {
        selector(behaviourTreeAI bot) : base(bot)
        {

        }
        public void addChild(task Task)
        {
            childTasks.Add(Task);
        }

        public override void setResult(bool result)
        {
            if(result)
            {
                isFinished = true;
            }
        }

        public override IEnumerator runTask()
        {
            foreach(task t in childTasks)
            {
                yield return StartCoroutine(t.runTask());
            }
        }
    }

    public class sequence : task
    {
        sequence(behaviourTreeAI bot) : base(bot)
        {

        }

        public override void setResult(bool result)
        {
            if (result)
            {
                isFinished = true;
            }
        }

        public void addChild(task Task)
        {
            childTasks.Add(Task);
        }

        public override IEnumerator runTask()
        {
            foreach (task t in childTasks)
            {
                StartCoroutine(t.runTask());

                if (t.getResult() == false)
                {
                    yield return false;
                }
            }
            yield return true;
        }
    }

}
