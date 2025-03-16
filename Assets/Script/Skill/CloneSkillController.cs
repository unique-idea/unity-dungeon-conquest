using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator animator;

    [SerializeField] private float ColorLosingSpeed;

    private float CloneTimmer;
    private float attackMultiplier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private Transform closesEnemy;
    private int facingDir = 1;

    private bool canDuplicateClone;
    private float chanceToDuplicate;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        CloneTimmer -= Time.deltaTime;
        if(CloneTimmer < 0)
        {
            sr.color = new Color(1,1,1, sr.color.a - (Time.deltaTime * ColorLosingSpeed));
        }
        if(sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void SetUpClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closesEnemy, bool _canDuplicateClone, float _chanceToDuplicate, Player _player, float _attackMultiplier)
    {
        if (_canAttack)
        {
            animator.SetInteger("AttackNumber", UnityEngine.Random.Range(1, 3));
        }

        attackMultiplier = _attackMultiplier;
        player = _player;
        transform.position = _newTransform.position + _offset;
        CloneTimmer = _cloneDuration;

        closesEnemy = _closesEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;
        FaceClosetTarget();
        AudioManager.instance.PlaySFX(21, null);
    }

    private void AnimationTrigger()
    {
        CloneTimmer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                // player.stats.DoDamage(hit.GetComponent<CharacterStat>());
                hit.GetComponent<Entity>().SetUpKnockBackDir(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    ItemDataEquipment weponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                    if (weponData != null)
                    {
                        weponData.Effect(hit.transform);
                    }
                }

                if (canDuplicateClone)
                {
                    if(UnityEngine.Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceClosetTarget()
    {
        if (closesEnemy != null)
        {
            Debug.Log("Face enemy");
            if (transform.position.x > closesEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
