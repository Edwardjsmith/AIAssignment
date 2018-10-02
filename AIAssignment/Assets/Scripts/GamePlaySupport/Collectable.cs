using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public virtual void Collect(AgentData agentData)
    {
        gameObject.transform.parent = agentData.transform;

        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.layer = 0;
    }

    public virtual void Drop(AgentData agentData, Vector3 position)
    {
        gameObject.transform.parent = null;
        gameObject.transform.position = position;

        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("VisibleToAI");
    }
}
