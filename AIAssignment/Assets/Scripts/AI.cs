using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************************************************************
 * Write your core AI code in this file here. The private variable 'agentScript' contains all the agents actions which are listed
 * below. Ensure your code it clear and organised and commented.
 *
 *
*****************************************************************************************************************************/

/// <summary>
/// Implement your AI script here, you can put everything in this file, or better still, break your code up into multiple files
/// and call the script here in the Update method
/// </summary>
public class AI : MonoBehaviour
{
    // Gives access to important data about the AI agent (see above)
    private AgentData _agentData;
    // Gives access to the agent senses
    private Sensing _agentSenses;
    // gives access to the agents inventory
    private InventoryController _agentInventory;
    // This is the script containing the AI agents actions
    // e.g. agentScript.MoveTo(enemy);
    private AgentActions _agentActions;

    // Use this for initialization
    void Start ()
    {
        // Initialise the accessable script components
        _agentData = GetComponent<AgentData>();
        _agentActions = GetComponent<AgentActions>();
        _agentSenses = GetComponentInChildren<Sensing>();
        _agentInventory = GetComponentInChildren<InventoryController>();
    }

    // Update is called once per frame
    void Update ()
    {

    }
}