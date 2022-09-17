using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerVRHudPersonal : MonoBehaviour
{

    public float timeToLive = 2f;

    public GameObject enemyTargetGO;
    public TMP_Text enemyTargetText;

    public GameObject navigationTargetGO;
    public TMP_Text navigationTargetText;

    private bool enemyTarget = false;
    private bool navigationTarget = false;

    public void setEnemyTarget(string name)
    {
        enemyTargetGO.SetActive(true);
        navigationTargetGO.SetActive(false);
        enemyTargetText.text = name;
        enemyTarget = true;
    }
    public void setNavigationTarget(string name)
    {
        enemyTargetGO.SetActive(false);
        navigationTargetGO.SetActive(true);
        navigationTargetText.text = name;
        navigationTarget = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive < 0f) Destroy(gameObject);
    }
}
