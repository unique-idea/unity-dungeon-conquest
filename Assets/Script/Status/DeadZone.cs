using UnityEngine;

public class DeadZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterStat>() != null)
        {
            //Debug.Log("Found Player");
            if (collision.GetComponent<CharacterStat>() != null)
            {
               // Debug.Log("Character Stat Not Null");
            }
            collision.GetComponent<CharacterStat>().KillEntity();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
