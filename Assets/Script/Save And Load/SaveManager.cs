using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    private GameData gameData;
    public static SaveManager instance;
    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;

    private List<ISaveManager> saveManagers;
    private FileDataHanlder datahandler;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        datahandler = new FileDataHanlder(Application.persistentDataPath, fileName, encryptData);
        saveManagers = FindAllSaveManager();

        LoadGame();
    }

    [ContextMenu("Delete save file")]
    public void DeleteSaveData()
    {
        datahandler = new FileDataHanlder(Application.persistentDataPath, fileName, encryptData);
        datahandler.Delete();
    }
    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = datahandler.Load();
        if(this.gameData == null)
        {
            NewGame();
        }

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        datahandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManager()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
     }

    public  bool HashSaveData()
    {
        if(datahandler.Load() != null)
        {
            return true;
        }
        return false;
    }
}
