using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D cd;

    private Player player;

    private float freezeTimerDuration;
    private float returnSpeed = 20;

    private bool canRotate = true;
    private bool isReturnning;

    [Header("Pierce Infor")]
    private float amountOfPierce;

    [Header("Bounce Infor")]
    private float bounceSpeed;
    private bool isBouncing;
    private int amountOfBounce;
    private List<Transform> enemyTargets;
    private int targetIndex;

    [Header("Spin Infor")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCoolDown;

    private float spinDirection;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetUpSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimerDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimerDuration = _freezeTimerDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (amountOfPierce <= 0)
        {
            animator.SetBool("Rotation", true);
        }

        AudioManager.instance.PlaySFX(15, null);
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 3);
    }

    public void SetUpPierce(int _amountOfPierce)
    {
        amountOfPierce = _amountOfPierce;
    }
    public void SetUpBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;
        bounceSpeed = _bounceSpeed;

        enemyTargets = new List<Transform>();
    }

    public void SetUpSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCoolDown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCoolDown = _hitCoolDown;
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturnning = true;

    }
    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturnning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 0.5)
            {
                player.CatchTheSword();
            }
        }

        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                // transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturnning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCoolDown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                            AudioManager.instance.PlaySFX(14, null);
                        }
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTargets[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturnning = true;
                }

                if (targetIndex >= enemyTargets.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isReturnning)
        {
            return;
        }
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetUpTargetForBounce(collision);
        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

        player.stats.DoDamage(enemyStats);

        if (player.skill.sword.timeStopUnlocked)
        {
            enemy.FreezeTimeFor(freezeTimerDuration);
        }

        if (player.skill.sword.vulnurableUnlocked)
        {
            enemyStats.MakeVulnerableFor(freezeTimerDuration + 1);
        }

        ItemDataEquipment equipmentAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

        if (equipmentAmulet != null)
        {
            equipmentAmulet.Effect(enemy.transform);
        }
    }

    private void SetUpTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTargets.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (amountOfPierce > 0 && collision.GetComponent<Enemy>() != null)
        {
            amountOfPierce--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (isBouncing && enemyTargets.Count > 0)
        {
            return;
        }

        animator.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
