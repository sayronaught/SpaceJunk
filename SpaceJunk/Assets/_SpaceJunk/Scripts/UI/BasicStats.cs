using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicStats : MonoBehaviour
{
    public PlayerShip myShip;

    public Transform energyBar;
    public TMP_Text energyText;
    public Transform hpBar;
    public TMP_Text hpText;

    private float updateTiming = 0f;

    public void updateBasicStats()
    {
        energyBar.localScale = new Vector3(myShip.energy/myShip.energyMax, 1, 1);
        energyText.text = myShip.energy.ToString()+"/"+myShip.energyMax.ToString();
        hpBar.localScale = new Vector3(myShip.structureHP / myShip.structureHPMax, 1,1);
        hpText.text = myShip.structureHP.ToString()+"/"+myShip.structureHPMax.ToString();
        updateTiming = 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( updateTiming < 0f ) updateBasicStats();
        updateTiming -= Time.deltaTime;
    }
}
