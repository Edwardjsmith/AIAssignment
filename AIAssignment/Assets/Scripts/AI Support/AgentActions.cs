using UnityEngine;

public class AgentActions : MonoBehaviour
{
    private const string AttackAnimationTrigger = "Attack";

    private AgentData _agentData;
    // Gives access to the agent senses
    private Sensing _agentSenses;
    // gives access to the agents inventory
    private InventoryController _agentInventory;

    private UnityEngine.AI.NavMeshAgent _navAgent;
    private Animator _swordAnimator;

    private AiMoodIconController _agentMoodIndicator;
    public AiMoodIconController AiMoodIndicator
    {
        get { return _agentMoodIndicator; }
    }

    // Use this for initialization
    void Start()
    {
        _agentData = GetComponent<AgentData>();
        _agentSenses = GetComponentInChildren<Sensing>();
        _agentInventory = GetComponentInChildren<InventoryController>();
        _navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _swordAnimator = GetComponentInChildren<Animator>();
        _agentMoodIndicator = GetComponentInChildren<AiMoodIconController>();
    }

    private bool TestDestination(Vector3 destinationToTest, out Vector3 destination)
    {
        // Check we can move there
        UnityEngine.AI.NavMeshHit navHit;
        if (UnityEngine.AI.NavMesh.SamplePosition(destinationToTest, out navHit, Vector3.Distance(transform.position, destinationToTest), AgentData.AgentLayerMask))
        {
            destination = navHit.position;
            return true;
        }

        destination = Vector3.zero;
        return false;
    }

    // Move towards a target object
    public bool MoveTo(GameObject target)
    {
        if (target != null)
        {
            // Check we can move there
            Vector3 destination;
            if (TestDestination(target.transform.position, out destination))
            {
                _navAgent.destination = destination;
                return true;
            }
        }
        return false;
    }

    // Move towards a target location
    public bool MoveTo(Vector3 target)
    {
        // Check we can move there
        Vector3 destination;
        if (TestDestination(target, out destination))
        {
            _navAgent.destination = destination;
            return true;
        }

        return false;
    }

    public void MoveToRandomLocation()
    {
        // Choose a new direction
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _agentData.Speed;
        randomDirection += transform.position;

        // Check we can move there
        Vector3 destination;
        if (TestDestination(randomDirection, out destination))
        {
            _navAgent.destination = destination;
        }
    }

    public void CollectItem(GameObject item)
    {
        if (item != null)
        {
            if (_agentSenses.IsItemInReach(item))
            {
                // If its collectable add it to the inventory
                if (item.GetComponent<Collectable>() != null)
                {
                    item.GetComponent<Collectable>().Collect(_agentData);
                    _agentInventory.AddItem(item);
                }
            }
        }
    }

    /// <summary>
    /// Use an item stored in the inventory if it is stored there
    /// </summary>
    /// <param name="item">The item GameObject</param>
    /// <returns>true if the item was successfully used, false otherwise</returns>
    public void UseItem(GameObject item)
    {
        if (item != null)
        {
            // Check we actually have it and it's usable
            if (_agentInventory.HasItem(item.name) && item.GetComponent<IUsable>() != null)
            {
                _agentInventory.RemoveItem(item.name);
                item.GetComponent<IUsable>().Use(_agentData);
            }
        }
    }

    public void DropItem(GameObject item)
    {
            // Check we actually have it and its collectable
            if (_agentInventory.HasItem(item.name) && item.GetComponent<Collectable>() != null)
            {
                // Check just in front of us that we're not dropping inside an obstacle
                Vector3 targetPoint = gameObject.transform.position + gameObject.transform.forward;
                // Make sure we're testing a position on the ground
                targetPoint.y = 1.0f;

                Vector3 dropPosition;
                if (TestDestination(targetPoint, out dropPosition))
                {
                    // Make sure we keep the original y position of the item
                    dropPosition.y = item.transform.position.y;
                    _agentInventory.RemoveItem(item.name);

                    item.GetComponent<Collectable>().Drop(_agentData, dropPosition);
                }
            }
    }

    // Attack the enemy
    public void AttackEnemy(GameObject target)
    {
        Debug.Log("Attacking " + target.name);

        // But only if it is the enemy
        if (target.CompareTag(_agentData.EnemyTeamTag))
        {
            // Swing the sword
            _swordAnimator.SetTrigger(AttackAnimationTrigger);

            // Only do damage if we're within attack range
            if(_agentSenses.IsInAttackRange(target))
            {
                // We may not always hit
                if (UnityEngine.Random.value < _agentData.HitProbability)
                {
                    int actualDamage = _agentData.NormalAttackDamage;
                    // Tell the enemy we hit them
                    if (_agentData.IsPoweredUp)
                    {
                        actualDamage *= _agentData.PowerUpAmount;
                    }
                    // TODO: handle damage properly... events?
                    //enemy..TakeDamage(actualDamage);
                    target.GetComponent<AgentData>().TakeDamage(actualDamage);
                }
            }
        }
    }

    // Move in the opposite direction to the enemy
    public void Flee(GameObject enemy)
    {
        // Turn away from the threat
        transform.rotation = Quaternion.LookRotation(transform.position - enemy.transform.position);
        Vector3 runTo = transform.position + transform.forward * _navAgent.speed;

        //So now we've got a Vector3 to run to and we can transfer that to a location on the NavMesh with samplePosition.
        // stores the output in a variable called hit
        UnityEngine.AI.NavMeshHit navHit;

        // Check for a point to flee to
        UnityEngine.AI.NavMesh.SamplePosition(runTo, out navHit, _agentData.Speed, 1 << UnityEngine.AI.NavMesh.GetAreaFromName("Walkable"));
        _navAgent.SetDestination(navHit.position);
    }
}