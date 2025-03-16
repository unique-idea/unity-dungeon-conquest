using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType{
    Big, Medium, Small
}
public class EnemySlime : Enemy
{
    [Header("Slime specific")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimesToCreate;
    [SerializeField] private GameObject slimePrefabs;
    [SerializeField] private Vector2 minCreationVeclocity;
    [SerializeField] private Vector2 maxCreationVeclocity;

    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }   
    public SlimeStunState stunState { get; private set; }
    public SlimeDeadState deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        SetUpDefaultFacingDir(-1);

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);

        stunState = new SlimeStunState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Idle", this);

    }
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

        if(slimeType == SlimeType.Small)
        {
            return;
        }

        CreateSlimes(slimesToCreate, slimePrefabs);
    }

    private void CreateSlimes(int _amountOfSlime, GameObject _slimePrefab)
    {
        for(int i = 0; i <  _amountOfSlime; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<EnemySlime>().SetUpSlime(facingDir);
        }


    }

    public void SetUpSlime(int _facingDir)
    {
        if(_facingDir != facingDir)
        {
            Flip();
        }

        float xVelocity = Random.Range(minCreationVeclocity.x, maxCreationVeclocity.x);
        float yVelocity = Random.Range(minCreationVeclocity.y, maxCreationVeclocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -facingDir, yVelocity);

        Invoke("CancelKnockBack", 1f);
    }

    private void CancelKnockBack() => isKnocked = false;

}
