using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator animator;
    public string id;
    public bool activationStatus;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }
    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            ActivateCheckPoint();
        }

    }

    public void ActivateCheckPoint()
    {
       // Debug.Log("Run Activated");
        if(activationStatus == false )
        {
            AudioManager.instance.PlaySFX(5, transform);
        }

        activationStatus = true;
        animator.SetBool("active", true);
    }
}
