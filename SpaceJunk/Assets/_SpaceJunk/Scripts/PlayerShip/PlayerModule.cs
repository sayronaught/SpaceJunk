using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    public int upgradeHP = 1;
    public int upgradeEnergy = 1;

    private PhotonView myPV;

    [PunRPC]
    public void playModuleSound(float strain)
    {

        if (structureHP <= structureMaxHP * 0.4) // heavy strain
        {
            myStructureSound.clip = myShip.myGM.SoundBank.MetalStrainHigh[Random.Range(0, myShip.myGM.SoundBank.MetalStrainHigh.Count-1)];
            myStructureSound.volume = 1f;
            myStructureSound.pitch = Random.Range(0.8f, 1.2f);
            myStructureSound.Play();
            Debug.Log("H");
        }
        else if (structureHP <= structureMaxHP * 0.7) // med strain
        {
            myStructureSound.clip = myShip.myGM.SoundBank.MetalStrainMedium[Random.Range(0, myShip.myGM.SoundBank.MetalStrainMedium.Count-1)];
            myStructureSound.volume = 0.8f;
            myStructureSound.pitch = Random.Range(0.8f, 1.2f);
            myStructureSound.Play();
            Debug.Log("M");
        }
        else // low strain
        {
            myStructureSound.clip = myShip.myGM.SoundBank.MetalStrainLow[Random.Range(0, myShip.myGM.SoundBank.MetalStrainLow.Count-1)];
            myStructureSound.volume = 0.6f;
            myStructureSound.pitch = Random.Range(0.8f, 1.2f);
            myStructureSound.Play();
            Debug.Log("L");
        }
    }

    [PunRPC]
    public void updateModuleNetWork(float newHP,float newHPMax,float newCap)
    {
        structureHP = newHP;
        structureMaxHP = newHPMax;
        energyCapacity = newCap;
    }

    public void updateModule()
    {
        myPV.RPC("updateModuleNetWork", RpcTarget.All, structureHP, structureMaxHP, energyCapacity);
    }

    public void TestHullStrain(float CalculateHullStrain)
    {
        if ((Random.Range(1, 20) == 1)) structureHP++;
        if ( Random.Range(0f,1000f) <= CalculateHullStrain )
        { // percentage chance each hull part takes damage
            structureHP -= CalculateHullStrain * 0.1f;
            myPV.RPC("playModuleSound", RpcTarget.All,CalculateHullStrain);
        }
        if (structureHP > structureMaxHP) structureHP = structureMaxHP;
        if (structureHP < 0) structureHP = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("we have a hit");
        if ( collision.transform.tag == "EnemyAmmo" && PhotonNetwork.IsMasterClient )
        {
            structureHP -= 5;
            updateModule();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        var col = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
