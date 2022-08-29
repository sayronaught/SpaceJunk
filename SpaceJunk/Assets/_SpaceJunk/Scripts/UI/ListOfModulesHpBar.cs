using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ListOfModulesHpBar : MonoBehaviour
{
    public PlayerModule myMod;

    public Transform hpBar;
    public TMP_Text hpText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.localScale = new Vector3(myMod.structureHP / myMod.structureMaxHP, 1, 1);
        hpText.text = myMod.structureHP.ToString("F0") + "/" + myMod.structureMaxHP.ToString();
    }
}
