using UnityEngine;

// Add new AI moods here as needed
public enum AiMood { Idle, Attacking, Fleeing, Winning, Losing, Dead };

/// <summary>
/// Agent data
/// </summary>
public class AgentData : MonoBehaviour
{
    public enum Teams
    {
        RedTeam,
        BlueTeam,
    }
    public Teams EnemyTeam;
    public Teams FriendlyTeam;

    private string _enemyTeamTag = "";
    public string EnemyTeamTag
    {
        get
        {
            return _enemyTeamTag;
        }
    }

    private string _friendlyFlagName = "";
    public string FriendlyFlagName
    {
        get
        {
            return _friendlyFlagName;
        }
    }

    private string _enemyFlagName = "";
    public string EnemyFlagName
    {
        get
        {
            return _enemyFlagName;
        }
    }

    private GameObject _friendlyBase;
    public GameObject FriendlyBase
    {
        get { return _friendlyBase; }
        set { _friendlyBase = value; }
    }

    private string _friendlyTeamTag = "";
    public string FriendlyTeamTag
    {
        get
        {
            return _friendlyTeamTag;
        }
    }

    // Agent stats
    public int MaxHitPoints = 100;
    public float AttackRange = 2.0f;
    public int NormalAttackDamage = 10;
    public float HitProbability = 0.5f;
    public float PickUpRange = 1.0f;
    public int Speed = 200;
    public int ViewRange = 10;

    // Check for collisions with everything when checking for a random location for the wander function
    public const int AgentLayerMask = -1;

    // Our current health, this is public in order to aid debugging
    public int CurrentHitPoints;

    // What is the current mood of the AI agent
    private AiMood _aiMood;
    public AiMood AiMood
    {
        get { return _aiMood; }
        set { _aiMood = value; }
    }

    // Are we still alive
    private bool _isAlive = true;
    public bool IsAlive
    {
        get { return _isAlive; }
        set { _isAlive = value; }
    }

    // Get the powerup value
    private int _powerUp = 0;
    public int PowerUpAmount
    {
        get { return _powerUp; }
    }

    // Do we have a powerup
    public bool IsPoweredUp
    {
        get { return _powerUp > 0; }
    }

    private bool _hasFriendlyFlag = false;

    public bool HasFriendlyFlag
    {
        get { return _hasFriendlyFlag; }
        set { _hasFriendlyFlag = value; }
    }

    private bool _hasEnemyFlag = false;

    public bool HasEnemyFlag
    {
        get { return _hasEnemyFlag; }
        set { _hasEnemyFlag = value; }
    }

    // We've died
    public void Die()
    {
        _isAlive = false;
        _aiMood = AiMood.Dead;
        Destroy(gameObject);
    }

    // We've been hit
    public void TakeDamage(int damage)
    {
        if (CurrentHitPoints + damage > 0)
        {
            CurrentHitPoints -= damage;
        }
        else
        {
            CurrentHitPoints = 0;
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        if (CurrentHitPoints + healAmount > MaxHitPoints)
        {
            CurrentHitPoints = MaxHitPoints;
        }
        else
        {
            CurrentHitPoints += healAmount;
        }
    }

    public void PowerUp(int powerAmount)
    {
        _powerUp = powerAmount;
    }

    // Use this for initialization
    void Start ()
    {
        CurrentHitPoints = MaxHitPoints;

        switch(EnemyTeam)
        {
            case Teams.BlueTeam:
                _enemyTeamTag = Tags.BlueTeam;
                _enemyFlagName = Names.BlueFlag;
                _friendlyTeamTag = Tags.RedTeam;
                _friendlyFlagName = Names.RedFlag;
                break;
            case Teams.RedTeam:
                _enemyTeamTag = Tags.RedTeam;
                _enemyFlagName = Names.RedFlag;
                _friendlyTeamTag = Tags.BlueTeam;
                _friendlyFlagName = Names.BlueFlag;
                break;
        }
    }

    void Update()
    {
        // TODO: Maybe remove this
        if (CurrentHitPoints < MaxHitPoints / 2)
        {
            _aiMood = AiMood.Losing;
        }
        else
        {
            _aiMood = AiMood.Idle;
        }
    }
}