using UnityEngine;
using UnityEngine.UI;

public class PowerUpStateIconController : MonoBehaviour
{
    public Sprite PowerupSprite;
    public Sprite EmptySprite;

    private Image _icon;
    private AgentData _agentData;


    // Use this for initialization
    void Start ()
    {
        _agentData = GetComponentInParent<AgentData>();
        _icon = GetComponent<Image>();
    }

    private void OnGUI()
    {
        if(_agentData.IsPoweredUp)
        {
            _icon.sprite = PowerupSprite;
        }
        else
        {
            _icon.sprite = EmptySprite;
        }
    }
}
