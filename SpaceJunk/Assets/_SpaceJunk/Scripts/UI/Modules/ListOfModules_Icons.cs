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

    [Tooltip("Drag Energy Icon into this")]
    public GameObject IconEnergy;

    [Tooltip("Drag Structure Icon into this")]
    public GameObject IconStructure;
    
    private float timer;

    public void clickPatchKit()
    {
        myPM.myShip.energy -= 25f;
        myPM.myShip.Inventory.removeItem("PatchKit", 1);
        myPM.structureHP += 25f;
        myPM.updateModule();
        myPM.myShip.updateInventoryPlus();
    }

    public void clickEnergy()
    {
        myPM.myShip.energy -= 50f;
        myPM.myShip.Inventory.removeItem("EnergyCrystal", 5*myPM.upgradeEnergy);
        myPM.upgradeEnergy++;
        myPM.energyCapacity += 100f;
        myPM.updateModule();
        myPM.myShip.updateInventoryPlus();
    }

    public void clickStructure()
    {
        myPM.myShip.energy -= 100f;
        myPM.myShip.Inventory.removeItem("Metal", 100*myPM.upgradeHP);
        myPM.upgradeHP++;
        myPM.structureMaxHP += 100f;
        myPM.updateModule();
        myPM.myShip.updateInventoryPlus();
    }

    void checkPatchKit()
    {
        IconPatchKit.SetActive(true);
        if (myPM.myShip.energy < 25) IconPatchKit.SetActive(false);
        if ( myPM.structureHP > (myPM.structureMaxHP-25) ) IconPatchKit.SetActive(false);
        if ( myPM.myShip.Inventory.countItem("PatchKit") < 1 ) IconPatchKit.SetActive(false);
    }

    void checkEnergy()
    {
        IconEnergy.SetActive(true);
        if (myPM.myShip.energy < 50) IconEnergy.SetActive(false);
        if (myPM.myShip.Inventory.countItem("EnergyCrystal") < 5 * myPM.upgradeEnergy) IconEnergy.SetActive(false);
    }

    void checkStructure()
    {
        IconStructure.SetActive(true);
        if (myPM.myShip.energy < 100) IconStructure.SetActive(false);
        if (myPM.myShip.Inventory.countItem("Metal") < 100 * myPM.upgradeHP) IconStructure.SetActive(false);
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
                checkEnergy();
                checkStructure();
            }
        } else { // these modules are not turned on by the script above it in the chain
            IconPatchKit.SetActive(false);
        }
    }
}
