using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the health bar for the AI agent
/// </summary>
public class HealthBarUpdate : MonoBehaviour
{
    private Slider _healthBar;
    private AgentData _agentData;

    // Use this for initialization
    void Start ()
    {
        _agentData = GetComponentInParent<AgentData>();
        _healthBar = GetComponent<Slider>();

        // Make sure that the health bar will reflect changes to the agents max health
        _healthBar.maxValue = _agentData.MaxHitPoints;
    }

    // Update is called once per frame
    private void OnGUI()
    {
        _healthBar.value = _agentData.CurrentHitPoints;
    }
}
