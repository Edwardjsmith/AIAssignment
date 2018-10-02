using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAgentSpawner : MonoBehaviour
{
    public enum AiAgentNumber
    {
        TeamMemberOne,
        TeamMemberTwo,
        TeamMemberThree
    }

    public AiAgentNumber ThisAiAgentNumber;

    private GameObject AiAgentPrefabToSpawn;
    private int RespawnDelay = 5;

    private GameObject _newAiAgent;
    private string _aiAgentName;
    private bool _isSpawnScheduled = false;

    private TeamData teamData;

    // Use this for initialization
    public void Start()
    {
        teamData = transform.parent.GetComponent<TeamData>();
        AiAgentPrefabToSpawn = teamData.AiAgentPrefab;
        RespawnDelay = teamData.RespawnDelay;

        SetAiAgentName();
        SpawnObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (_newAiAgent == null && !_isSpawnScheduled)
        {
            StartCoroutine(SpawnDelay());
            _isSpawnScheduled = true;
        }
    }

    public void SetAiAgentName()
    {
        _aiAgentName = AiAgentPrefabToSpawn.name;

        switch (ThisAiAgentNumber)
        {
            case AiAgentNumber.TeamMemberOne:
                _aiAgentName += " 1";
                break;
            case AiAgentNumber.TeamMemberTwo:
                _aiAgentName += " 2";
                break;
            case AiAgentNumber.TeamMemberThree:
                _aiAgentName += " 3";
                break;
        }
    }

    protected IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(RespawnDelay);
        SpawnObject();
        _isSpawnScheduled = false;
        yield return null;
    }

    protected void SpawnObject()
    {
        _newAiAgent = Instantiate(AiAgentPrefabToSpawn, gameObject.transform.position, gameObject.transform.localRotation);
        _newAiAgent.name = _aiAgentName;
        _newAiAgent.GetComponent<AgentData>().FriendlyBase = gameObject.transform.parent.gameObject;
    }
}
