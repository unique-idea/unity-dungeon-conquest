using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParadoxBackGround : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float paradoxEffect;
    private float xPosition;
    private float lenght;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        float distanceMove = cam.transform.position.x * (1 - paradoxEffect);
        float distanceToMove = cam.transform.position.x * paradoxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if(distanceMove > xPosition + lenght)
        {
            xPosition = xPosition + lenght;
        }else if(distanceMove < xPosition - lenght)
        {
            xPosition = xPosition - lenght;
        }
    }
}
