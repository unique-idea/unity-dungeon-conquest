using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadVoid : Enemy
{
    public bool bossFightBegun;

    [Header("Teleport details")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheck;
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 20;

    [Header("Spell cast details")]
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private float spellStateCoolDown;
    public float lastTimeCast;
    public int amountOfSpell;
    public float spellCoolDown;

    public DeadVoidBattleState battleState { get; private set; }
    public DeadVoidAttackState attackState { get; private set; }
    public DeadVoidIdleState idleState { get; private set; }
    public DeadVoidDeadState deadState { get; private set; }
    public DeadVoidSpellCastState spellCastState { get; private set; }
    public DeadVoidTeleportState teleportState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        SetUpDefaultFacingDir(-1);

        idleState = new DeadVoidIdleState(this, stateMachine, "Idle", this);
        battleState = new DeadVoidBattleState(this, stateMachine, "Move", this);
        attackState = new DeadVoidAttackState(this, stateMachine, "Attack", this);

        deadState = new DeadVoidDeadState(this, stateMachine, "Idle", this);
        spellCastState = new DeadVoidSpellCastState(this, stateMachine, "SpellCast", this);
        teleportState = new DeadVoidTeleportState(this, stateMachine, "Teleport", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;

        float xOffset = 0;

        if (player.rb.velocity.x != 0)
        {
            xOffset = player.facingDir * 3;
        }

        Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + 1.5f);

        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
        AudioManager.instance.PlaySFX(28 ,null);
        newSpell.GetComponent<DeadVoidSpellController>().SetUpSpell(stats);
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomeThingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheck, 0, Vector2.zero, 0, whatIsGround);
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawCube(transform.position, surroundingCheck);
    }

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

        if(!GroundBelow() || SomeThingIsAround())
        {
            FindPosition();
        }
    }

    public bool CanTeleport()
    {
        if(Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }
        return false;
    }

    public bool CanDoSpellCast()
    {
      /*  Debug.Log("Last time cast :" + lastTimeCast + " spellCoolDown: " + spellCoolDown);
        Debug.Log("Spell time: " + (lastTimeCast + spellCoolDown));
        Debug.Log("Time: " + Time.time); */
        if(Time.time >= lastTimeCast + spellStateCoolDown)
        {
            lastTimeCast = Time.time;
            return true;
        }

        return false;
    }
}
