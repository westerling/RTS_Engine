using Mirror;
using System;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public delegate void HealthChangedDelegate(int currentHealth, int maxHealth);
    public event HealthChangedDelegate EventHealthChanged;
    public event Action ServerOnDie;

    [SerializeField]
    [SyncVar]
    private int m_CurrentHealth;

    [SerializeField]
    [SyncVar]
    private int m_MaxHealth = 1;

    private Stats m_Stats;

    public Stats Stats
    {
        get => m_Stats;
        set => m_Stats = value;
    }

    public int MaxHealth
    {
        get => m_MaxHealth;
        set => m_MaxHealth = value;
    }

    public int CurrentHealth
    {
        get => m_CurrentHealth;
        set => m_CurrentHealth = value;
    }

    public bool HasFullHealth()
    {
        if (CurrentHealth >= MaxHealth)
        {
            return true;
        }

        return false;
    }

    #region server
    public override void OnStartServer()
    {
        Stats = GetComponent<LocalStats>().Stats;
        AlterMaxHealth();
        
        UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;
        GetComponent<LocalStats>().StatsAltered += HandleAlteredStats;
    }

    public override void OnStopServer()
    {
        UnitBase.ServerOnPlayerDie -= ServerHandlePlayerDie;
        GetComponent<LocalStats>().StatsAltered -= HandleAlteredStats;
    }

    [Command]
    private void CmdAlterMaxHealth()
    {
        AlterMaxHealth();
    }

    [Server]
    private void AlterMaxHealth()
    {
        var newMaxHealth = (int)Stats.GetAttributeAmount(AttributeType.HitPoints);
        
        if (CurrentHealth == MaxHealth || 
            CurrentHealth == 0)
        {
            MaxHealth = newMaxHealth;
            SetHealth(MaxHealth);
        }

        var percentage = CurrentHealth / MaxHealth;

        MaxHealth = newMaxHealth;
        CurrentHealth *= percentage;
    }

    [Server]
    public void DealDamage(int attackAmount, int attackStyle)
    {
        if (CurrentHealth == 0)
        {
            return;
        }

        var damageDealt = 1;

        switch ((AttackStyle)attackStyle)
        {
            case AttackStyle.Melee:
                damageDealt = (int)Math.Max(1, (Math.Max(0, attackAmount - Stats.GetAttributeAmount(AttributeType.MeleeArmor))));
                break;
            case AttackStyle.Pierce:
                damageDealt = (int)Math.Max(1, (Math.Max(0, attackAmount - Stats.GetAttributeAmount(AttributeType.PierceArmor))));
                break;
            case AttackStyle.None:
            default:
                damageDealt = attackAmount;
                break;
        }
        var health = Mathf.Max(CurrentHealth - damageDealt, 0);
        SetHealth(health);

        if (CurrentHealth != 0)
        {
            return;
        }

        ServerOnDie?.Invoke();
    }

    [Command]
    public void CmdSetMaxHealth()
    {
        SetMaxHealth();
    }

    [Server]
    public void SetMaxHealth()
    {
        CurrentHealth = MaxHealth;
    }

    [Command]
    public void CmdSetHealth(int health)
    {
        SetHealth(health);
    }

    [Server]
    public void SetHealth(int health)
    {
        CurrentHealth = health;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        EventHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    [Server]
    private void ServerHandlePlayerDie(int id)
    {
        if (connectionToClient.connectionId != id)
        {
            return;
        }

        DealDamage(CurrentHealth, 0);
    }
    #endregion

    #region client

    private void HandleAlteredStats(Stats stats)
    {
        Stats = stats;
        CmdAlterMaxHealth();
    }

    #endregion
}
