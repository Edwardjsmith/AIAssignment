using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject ObjectPrefabToSpawn;
    public int RespawnDelay = 5;

    private GameObject _newObject;
    private string _objectName;
    private bool _isSpawnScheduled = false;

    // Use this for initialization
    public void Start ()
	{
        SetObjectName();
        SpawnObject();
	}

    public void SetObjectName()
    {
        _objectName = ObjectPrefabToSpawn.name;
    }

    // Update is called once per frame
    void Update ()
	{
	    if (_newObject == null && !_isSpawnScheduled)
	    {
	        StartCoroutine(SpawnDelay());
	        _isSpawnScheduled = true;
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
        _newObject = Instantiate(ObjectPrefabToSpawn, gameObject.transform.position, gameObject.transform.localRotation);
        _newObject.name = _objectName;
    }
}
