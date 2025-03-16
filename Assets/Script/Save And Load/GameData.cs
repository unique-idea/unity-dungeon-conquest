using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class GameData
{
    public int currency;

    public SerizalbleDictionary<string, bool> skillTree;
    public SerizalbleDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerizalbleDictionary<string, bool> checkPoints;
    public string closestCheckPointId;

    public float lostCurencyX;
    public float lostCurencyY;
    public int lostCurencyAmount;

    public SerizalbleDictionary<string, float> volumeSettings;
    public GameData()
    {
        this.lostCurencyX = 0;
        this.lostCurencyY = 0;
        this.lostCurencyAmount = 0;

        this.currency = 7000;
        skillTree = new SerizalbleDictionary<string, bool>();
        inventory = new SerizalbleDictionary<string, int>();
        equipmentId = new List<string>();


        closestCheckPointId = string.Empty;
        checkPoints = new SerizalbleDictionary<string, bool>();

        volumeSettings = new SerizalbleDictionary<string, float>();
    }
}
