using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    private Transform player;

    [SerializeField] private CheckPoint[] checkPoints;
    private string closestCheckPointId;

    [Header("Lost Currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
        checkPoints = FindObjectsOfType<CheckPoint>();
    }
    private void Start()
    {
        checkPoints = FindObjectsOfType<CheckPoint>();

        player = PlayerManager.instance.player.transform;
    }
    public void ReStartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelay(_data));
    }

    private void LoadCheckPoint(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkPoints)
        {
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.id == pair.Key && pair.Value == true)
                {
                    checkPoint.ActivateCheckPoint();
                }
            }
        }
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        LoadCheckPoint(_data);
        LoadClosestCheckpoint(_data);
        LoadLostCurrency(_data);

    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurencyAmount;
        lostCurrencyX = _data.lostCurencyX;
        lostCurrencyY = _data.lostCurencyY;

        if(lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency =Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }
    private void LoadClosestCheckpoint(GameData _data)
    {
        if(_data.closestCheckPointId == null)
        {
            return;
        }

        closestCheckPointId = _data.closestCheckPointId;

        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (closestCheckPointId == checkPoint.id)
            {
                player.transform.position = checkPoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurencyAmount = lostCurrencyAmount;
        _data.lostCurencyX = player.position.x;
        _data.lostCurencyY = player.position.y;

        _data.closestCheckPointId = FindClosetsCheckPoint()?.id;
        _data.checkPoints.Clear();
        foreach (CheckPoint checkPoint in checkPoints)
        {
            _data.checkPoints.Add(checkPoint.id, checkPoint.activationStatus);
        }
    }

    private CheckPoint FindClosetsCheckPoint()
    {
        float closetDistance = Mathf.Infinity;
        CheckPoint closetCheckPoint = null;

        foreach (var checkPoint in checkPoints)
        {
            float distanceToCheckPoint = Vector2.Distance(player.transform.position, checkPoint.transform.position);

            if (distanceToCheckPoint < closetDistance && checkPoint.activationStatus == true)
            {
                closetDistance = distanceToCheckPoint;
                closetCheckPoint = checkPoint;
            }
        }

        return closetCheckPoint;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}


