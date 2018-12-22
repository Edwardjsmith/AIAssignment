using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Node {

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
            
            if(ai.getSenses().IsInAttackRange(ai.getTargetObj()))
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
