using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterStat>() != null)
        {
            AudioManager.instance.StopAllBGM();
            SceneManager.LoadScene(sceneName);

        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
