using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScore : MonoBehaviour
{
    public GameObject EnemyFlag;
    public int Score;
    private bool _enemyFlagInBase;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if(other.gameObject.name.Equals(EnemyFlag.name))
        {
            _enemyFlagInBase = true;
            StartCoroutine(UpdateScore());
        }
    }

    void OnTriggerExit(Collider other)
    {
        _enemyFlagInBase = false;
    }

    IEnumerator UpdateScore()
    {
        while(_enemyFlagInBase)
        {
            yield return new WaitForSeconds(1.0f);
            Score++;
        }
    }
}
