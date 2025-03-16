using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFadedScreen : MonoBehaviour
{
   private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

   public void FadeOut()
    {
        animator.SetTrigger("fadeOut");
    }

    public void FadeIn()
    {
        animator.SetTrigger("fadeIn");
    }
}
