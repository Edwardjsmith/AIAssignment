using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    public GameObject FriendlyBase;
    private SetScore _scoreData;
    private Text _scoreDisplayText;

    // Use this for initialization
    void Start ()
    {
        _scoreData = FriendlyBase.GetComponent<SetScore>();
        _scoreDisplayText = gameObject.GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnGUI()
    {
        _scoreDisplayText.text = _scoreData.Score.ToString();
    }
}
