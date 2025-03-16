using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("End screen")]
    [SerializeField] private UIFadedScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    public UIItemToolTip itemToolTip;
    public UIStatToolTip statToolTip;
    public UICraftWindow craftWindow;
    public UISkillToolTip skillToolTip;

    [SerializeField] private UIVolumeSlider[] volumeSettings;


    private void Awake()
    {
        //Use to assign events on skill tree slots be4 we assign events on skill scripts
        if (skillTreeUI == null)
        {
            Debug.Log("SKill tree is null");
        }
        SwitchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true);
    }
    void Start()
    {
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        //itemToolTip = GetComponentInChildren<UIItemToolTip>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SWitchWithKeyTo(characterUI);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SWitchWithKeyTo(craftUI);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SWitchWithKeyTo(skillTreeUI);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SWitchWithKeyTo(optionsUI);
        }
    }

    public void SwitchTo(GameObject _menu)
    {
        // Debug.Log("Menu in switch to :" + _menu);
        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UIFadedScreen>() != null; // keep the fade screen active
            if (fadeScreen == false)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            /*   if (_menu != null)
               {
                   if (transform.GetChild(i).gameObject.name == _menu.name)
                   {
                       transform.GetChild(i).gameObject.SetActive(true);
                   }
               } */
        }

        if (_menu != null)
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(7, null);
            }
            _menu.SetActive(true);
        }

        if (GameManager.instance != null)
        {
            if (_menu == inGameUI)
            {
                GameManager.instance.PauseGame(false);
            }
            else
            {
                GameManager.instance.PauseGame(true);
            }
        }
    }

    public void SWitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UIFadedScreen>() == null)
            {
                return;
            }
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEdnScreen()
    {
        //Debug.Log("Run into switch");
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutine());
    }

    IEnumerator EndScreenCorutine()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void ReStartGameButton()
    {
       // Debug.Log("Restart");
        GameManager.instance.ReStartScene();
        AudioManager.instance.StopSFX(31);
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach (UIVolumeSlider item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                {
                    item.LoadSlider(pair.Value);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (UIVolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
