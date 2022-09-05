using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModule : MonoBehaviour
{
    public string moduleName = "Standard Name";
    public string moduleDescription = "Standard desc";

    public float myMass = 500f; //500kg as standard

    public List<Transform> Ports;
    public List<PlayerStation> Stations;
    public List<PlayerModule> ModulesConnected;

    public PlayerShip myShip;
    public AudioSource myStructureSound;

    public float structureHP = 50f;
    public float structureMaxHP = 100f;
    public float energyCapacity = 100f;

    private PhotonView myPV;

    [PunRPC]
    public void playModuleSound(float strain)
    {
        
        if (structureHP <= structureMaxHP * 0.4) // heavy strain
        {
            myStructureSound.clip = myShip.myGM.SoundBank.MetalStrainHigh[Random.Range(0, myShip.myGM.SoundBank.MetalStrainHigh.Count)];
            myStructureSound.Play();
            Debug.Log("H");
        }
        else if (structureHP <= structureMaxHP * 0.7) // med strain
        {
            myStructureSound.clip = myShip.myGM.SoundBank.MetalStrainHigh[Random.Range(0, myShip.myGM.SoundBank.MetalStrainMedium.Count)];
            myStructureSound.Play();
            Debug.Log("M");
        }
        else // low strain
        {
            myStructureSound.clip = myShip.myGM.SoundBank.MetalStrainHigh[Random.Range(0, myShip.myGM.SoundBank.MetalStrainLow.Count)];
            myStructureSound.Play();
            Debug.Log("L");
        }
    }

    public void TestHullStrain(float CalculateHullStrain)
    {
        if ((Random.Range(1, 6) == 1)) structureHP++;
        if ( Random.Range(0f,1000f) <= CalculateHullStrain )
        { // percentage chance each hull part takes damage
            structureHP -= CalculateHullStrain * 0.1f;
            myPV.RPC("playModuleSound", RpcTarget.All,CalculateHullStrain);
        }
        if (structureHP > structureMaxHP) structureHP = structureMaxHP;
        if (structureHP < 0) structureHP = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
