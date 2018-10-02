using UnityEngine;

public class Flag : Collectable
{
    private const float flagRaiseAmount = 1;

    public override void Collect(AgentData agentData)
    {
        gameObject.transform.parent = agentData.transform;
        gameObject.GetComponent<BoxCollider>().enabled = false;

        Vector3 flagPosition = gameObject.transform.position;
        flagPosition.y += flagRaiseAmount;
        gameObject.transform.position = flagPosition;

        gameObject.layer = 0;

        agentData.HasFriendlyFlag = true;
    }

    public override void Drop(AgentData agentData, Vector3 position)
    {
        Debug.Log("Drop the flag");

        gameObject.transform.parent = null;
        gameObject.GetComponent<BoxCollider>().enabled = true;

        //gameObject.transform.position = position;
        Vector3 flagPosition = position;
        flagPosition.y -= flagRaiseAmount;
        gameObject.transform.position = flagPosition;

        gameObject.layer = LayerMask.NameToLayer("VisibleToAI");

        agentData.HasFriendlyFlag = false;

    }
}
