using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles the AI agents sensory suite, uses sphere collider to detect objects in view range, provides access functions to percieved objects
/// and a property to access the list holding detected objects
/// </summary>
public class Sensing : MonoBehaviour
{
    // The owner of the senses
    private AgentData _agentData;

    private const int MaxObjectsInView = 10;
    public LayerMask ObjectsToSeeMask;

    // Keep track of game objects in our visual field
    private readonly Dictionary<String, GameObject> _objectsPercieved = new Dictionary<String, GameObject>();
    public Dictionary<String, GameObject> ObjectsPercieved
    {
        get { return _objectsPercieved; }
    }

    // Use this for initialization
    void Start()
    {
        _agentData = GetComponentInParent<AgentData>();
    }

    private Collider[] _overlapResults = new Collider[MaxObjectsInView];
    private List<GameObject> _objectsInView = new List<GameObject>(MaxObjectsInView);

    void Update()
    {
        // Get objects in view
        int numFound = Physics.OverlapSphereNonAlloc(transform.position, _agentData.ViewRange, _overlapResults, ObjectsToSeeMask);
        _objectsInView.Clear();

        // Add all except ourselves to list of GameObjects in view range
        for (int i = 0; i < numFound; i++)
        {
            if (!_overlapResults[i].gameObject.name.Equals(gameObject.transform.parent.name))
            {
                _objectsInView.Add(_overlapResults[i].gameObject);
            }
        }
    }

    public List<GameObject> GetObjectsInView()
    {
        return _objectsInView;
    }

    public List<GameObject> GetCollectablesInView()
    {
        return _objectsInView.Where(x => x.CompareTag(Tags.Collectable)).ToList();
    }

    public List<GameObject> GetFriendliesInView()
    {
        return _objectsInView.Where(x => x.CompareTag(_agentData.FriendlyTeamTag)).ToList();
    }

    public List<GameObject> GetEnemiesInView()
    {
        return _objectsInView.Where(x => x.CompareTag(_agentData.EnemyTeamTag)).ToList();
    }

    public List<GameObject> GetObjectsInViewByTag(string tagToSelect)
    {
        return _objectsInView.Where(x => x.CompareTag(tagToSelect)).ToList();
    }

    public GameObject GetObjectInViewByName(string nameToSelect)
    {
        return _objectsInView.SingleOrDefault(x=>x.name.Equals(nameToSelect));
    }

    // Check if something of interest is in range
    public bool IsItemInReach(GameObject item)
    {
        if (item != null)
        {
            if (Vector3.Distance(gameObject.transform.parent.position, item.transform.position) < _agentData.PickUpRange)
            {
                return true;
            }
        }

        return false;
    }

    // Check if we're with attacking range of the enemy
    public bool IsInAttackRange(GameObject target)
    {
        // Get the game object from the name
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < _agentData.AttackRange)
            {
                return true;
            }
        }
        return false;
    }
}