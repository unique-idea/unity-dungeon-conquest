using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "SampleScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UIFadedScreen fadedScreen;

    private void Start()
    {
        if(SaveManager.instance.HashSaveData() == false)
        {
            continueButton.SetActive(false);
        }
        AudioManager.instance.PlayBGM(0);
    }

    public void ContinueGame()
    {
        AudioManager.instance.PlayBGM(1);
        StartCoroutine(LoadSceneWithFadeEffect(1f));
    }

    public void NewGame()
    {
        //Debug.Log("Run New Game");
        AudioManager.instance.PlayBGM(1);
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadSceneWithFadeEffect(1f));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadedScreen.FadeOut();

        yield return new WaitForSeconds(_delay);
        AudioManager.instance.StopAllBGM();
        SceneManager.LoadScene(sceneName);
    }

}
