using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
public enum StatType
{
    strength,
    agility,
    intelegence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightingDamage
}
public class CharacterStat : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength; //1 point increase damage by 2 and crit power by 1%
    public Stat agility; // 1 point increase evasion by 1% and crit chance by 1%
    public Stat intelligence; // 1 point increase magic dame by 1 and magic resistance by 3
    public Stat vitality; // 1 point increase health by 3 or 5 points

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;

    public bool isIgnited; //Does damage over time
    public bool isChilled; // reduce enemy armor by %
    public bool isShocked; // reduce accuracy by %

    [SerializeField] private float elementDuration = 3;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject thunderStrikePrefab;
    private int shockDamage;

    public int currentHealth;
    public bool isDead { get; private set; }
    private bool isVulnerable;

    public System.Action onHealthChanged;
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if (chilledTimer < 0)
        {
            isChilled = false;
        }

        if (shockedTimer < 0)
        {
            isShocked = false;
        }

        if (isIgnited)
        {
            ApplyIgniteDamage();
        }

    }

    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableCoroutine(_duration));
    }

    private IEnumerator VulnerableCoroutine(float _duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duration);

        isVulnerable = false;
    }
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModiCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModiCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0 )
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth <= 0 && !isDead)
            {
                Die();
            }
            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public void SetUpShockDamage(int _damage) => shockDamage = _damage;
    public void SetUpIgniteDamage(int _damage) => igniteDamage = _damage;

    public virtual void DoDamage(CharacterStat _targetStats)
    {
        bool criticalStrike = false;

        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }
        _targetStats.GetComponent<Entity>().SetUpKnockBackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            criticalStrike = true;
        }

        fx.CreateHitFx(_targetStats.transform, criticalStrike);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats); //if inventory have a item that have a element effect
    }

    public virtual void DoMagicalDamage(CharacterStat _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);

        _targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return;
        }
        AttempTypeToApplyElement(_targetStats, _fireDamage, _iceDamage, _lightingDamage);

    }

    private  void AttempTypeToApplyElement(CharacterStat _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyElement(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyElement(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyElement(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }
        if (canApplyShock)
        {
            _targetStats.SetUpShockDamage(Mathf.RoundToInt(_lightingDamage * .1f));
        }

        if (canApplyIgnite)
        {
            _targetStats.SetUpIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }

        _targetStats.ApplyElement(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private int CheckTargetResistance(CharacterStat _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyElement(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = elementDuration;

            fx.IgniteFxFor(elementDuration);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = elementDuration;

            float slowPercentage = .2f;

            GetComponent<Entity>().SlowEnemyBy(slowPercentage, elementDuration);
            fx.ChillFxFor(elementDuration);
        }

        if (_shock && canApplyShock)
        {
            if(!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                HitNearestTargetWithThunderStrike();
            }
        }
       
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
        {
            return;
        }

        isShocked = _shock;
        shockedTimer = elementDuration;

        fx.ShockFxFor(elementDuration);
    }

    private void HitNearestTargetWithThunderStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closetDistance = Mathf.Infinity;
        Transform closesEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closetDistance)
                {
                    closetDistance = distanceToEnemy;
                    closesEnemy = hit.transform;
                }
            }
        }
        if (closesEnemy == null)
        {
            closesEnemy = transform;
        }

        if (closesEnemy != null)
        {
            GameObject newThunderStrike = Instantiate(thunderStrikePrefab, transform.position, Quaternion.identity);

            newThunderStrike.GetComponent<ThunderStrikeController>().SetUp(shockDamage, closesEnemy.GetComponent<CharacterStat>());
        }
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if(currentHealth > GetMaxHealthValue())
        {
            currentHealth = GetMaxHealthValue();
        }

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
        {
            _damage = Mathf.RoundToInt(_damage * 1.1f);
        }
        currentHealth -= _damage;

        if(_damage > 0)
        {
            fx.CreatePopUpText(_damage.ToString());
        }

        if(onHealthChanged != null)
        {
            onHealthChanged();
        }
    }
    public virtual void OnEvasion()
    {

    }
    protected bool TargetCanAvoidAttack(CharacterStat _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }

    protected int CheckTargetArmor(CharacterStat _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;

    }

    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        
        return false;
    }

    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + agility.GetValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    public Stat GetStat(StatType _statType)
    {
        if (_statType == StatType.strength)
        {
            return strength;
        }
        if (_statType == StatType.agility)
        {
            return agility;
        }
        if (_statType == StatType.intelegence)
        {
            return intelligence;
        }
        if (_statType == StatType.vitality)
        {
            return vitality;
        }
        if (_statType == StatType.damage)
        {
            return damage;
        }
        if (_statType == StatType.critChance)
        {
            return critChance;
        }
        if (_statType == StatType.critPower)
        {
            return critPower;
        }
        if (_statType == StatType.health)
        {
            return maxHealth;
        }
        if (_statType == StatType.armor)
        {
            return armor;
        }
        if (_statType == StatType.evasion)
        {
            return evasion;
        }
        if (_statType == StatType.magicRes)
        {
            return magicResistance;
        }
        if (_statType == StatType.fireDamage)
        {
            return fireDamage;
        }
        if (_statType == StatType.iceDamage)
        {
            return iceDamage;
        }
        if (_statType == StatType.lightingDamage)
        {
            return lightingDamage;
        }

        return null;
    }

    public void KillEntity()
    {
        //Debug.Log("is Dead : " + isDead);
        if (!isDead)
        {
           Die();
        }
    }
}
