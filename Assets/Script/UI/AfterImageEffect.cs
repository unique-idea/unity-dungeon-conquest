using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    private float colorLoeseRate;

    public void SetUpAfterImage(float _loosingSpeed, Sprite _spriteImage)
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = _spriteImage;
        colorLoeseRate = _loosingSpeed;
    }

    private void Update()
    {
       float alpha = sr.color.a  - colorLoeseRate * Time.deltaTime;
       sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
       
         if(sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
