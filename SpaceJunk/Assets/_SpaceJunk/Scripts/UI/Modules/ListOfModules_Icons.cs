using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfModules_Icons : MonoBehaviour
{
    [Tooltip("This will get turned on by the script that adds this list item")]
    public bool turnedOn = false;

    [Tooltip("This will be set by the script that adds this list item")]
    public PlayerModule myPM;

    [Tooltip("Drag PatchKit Icon into this")]
    public GameObject IconPatchKit;

    private float timer;

    public void clickPatchKit()
    {
        //removeItem(string removeName, int num)
    }

    void checkPatchKit()
    {
        IconPatchKit.SetActive(true);
        if (myPM.myShip.energy < 25) IconPatchKit.SetActive(false);
        if ( myPM.structureHP > (myPM.structureMaxHP-25) ) IconPatchKit.SetActive(false);
        if ( myPM.myShip.Inventory.countItem("PatchKit") < 1 ) IconPatchKit.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = 1f;
        if ( turnedOn )
        { // these icons will check module to see which should be on
            if ( myPM )
            { // link to module is ot broken
                checkPatchKit();
            }
        } else { // these modules are not turned on by the script above it in the chain
            IconPatchKit.SetActive(false);
        }
    }
}
