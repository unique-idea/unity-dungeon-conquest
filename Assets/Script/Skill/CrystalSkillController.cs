
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator animator => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;

    private float crystalExitTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;
    private Transform closesTarget;
    [SerializeField] private LayerMask whatIsEnemy;
    public void SetUpCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closesTarget, Player _player)
    {
        crystalExitTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closesTarget = _closesTarget;
        player = _player;
    }

    public void ChooseRandomEnemy()
    {

        float radius = SkillManager.instance.blackHole.GetBlackHoleRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if(colliders.Length > 0 )
        {

        closesTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }

    }
    private void Update()
    {
        crystalExitTimer -= Time.deltaTime;

        if(crystalExitTimer < 0)
        {
            FinishCrystal();
            canMove = false;
        }

        if(canMove)
        {
            if(closesTarget == null)
            {
                return;
            }

            transform.position = Vector2.MoveTowards(transform.position, closesTarget.position, moveSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, closesTarget.position) < 1)
            {
                FinishCrystal();
            }
        }

        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3,3), growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetUpKnockBackDir(transform);
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStat>());

                ItemDataEquipment equipmentAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if(equipmentAmulet != null)
                {
                    equipmentAmulet.Effect(hit.transform);
                }
            }
        }
    }
    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            animator.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
