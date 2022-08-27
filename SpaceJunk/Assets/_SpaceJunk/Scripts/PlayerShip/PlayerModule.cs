using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModule : MonoBehaviour
{
    public string moduleName = "Standard Name";
    public string moduleDescription = "Standard desc";

    public List<Transform> Ports;
    public List<PlayerStation> Stations;
    public List<PlayerModule> ModulesConnected;

    public PlayerShip myShip;

    public float structureHP = 50f;
    public float structureMaxHP = 100f;
    public float energyCapacity = 100f;

    public void TestHullStrain(float CalculateHullStrain)
    {
        if ((Random.Range(1, 6) == 6)) structureHP++;
        if ( Random.Range(0f,100f) <= CalculateHullStrain )
        { // percentage chance each hull part takes damage
            structureHP -= CalculateHullStrain;
        }
        if (structureHP > structureMaxHP) structureHP = structureMaxHP;
        if (structureHP < 0) structureHP = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
