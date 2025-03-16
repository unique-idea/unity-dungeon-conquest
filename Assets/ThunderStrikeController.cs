using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{

    [SerializeField] private CharacterStat targetStats;
    [SerializeField] private float speed;
    private int damage;


    private bool trigger;
    private Animator animator;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetUp(int _damage, CharacterStat _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }
    void Update()
    {
        if (!targetStats)
        {
            return;
        }

        if(trigger)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if(Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            animator.transform.localPosition = new Vector3(0, .5f);
            transform.localRotation = Quaternion.identity;

            animator.transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            AudioManager.instance.PlaySFX(16, null);
            Invoke("DamageAndSelfDestroy", .2f);

            trigger = true;
            animator.SetTrigger("Hit");

        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }
}
