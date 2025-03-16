using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    private TextMeshPro myText;

    [SerializeField] private float speed;
    [SerializeField] private float disapiearingSpeed;
    [SerializeField] private float colorDisapiearingSpeed;

    [SerializeField] private float lifeTime;

    private float textTimer;
    void Start()
    {
        myText = GetComponent<TextMeshPro>();
        textTimer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);
        textTimer -= Time.deltaTime;

        if(textTimer < 0)
        {
            float alpha = myText.color.a - colorDisapiearingSpeed * Time.deltaTime;

            myText.color = new Color(myText.color.r, myText.color.g, alpha);

            if(myText.color.a < 50)
            {
                speed = disapiearingSpeed;
            }

            if(myText.color.a < 50)
            {
                Destroy(gameObject);
            }
        }
    }
}
