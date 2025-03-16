using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHockeyController : MonoBehaviour
{
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;
    private SpriteRenderer sr;

    private Transform myEnemy;
    private BlackHoleController blackHole;

    public void SetUpHotKey(KeyCode _myNewHotKey, Transform _myEnemy, BlackHoleController _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();

    }

    private void Update()
    {
        if(Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
