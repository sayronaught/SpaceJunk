using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicStats : MonoBehaviour
{
    public PlayerShip myShip;

    public Transform energyBar;
    public TMP_Text energyText;

    public void updateBasicStats()
    {
        energyBar.localScale = new Vector3(0.25f, 1, 1);
        energyText.text = "500/1000";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateBasicStats();
    }
}
