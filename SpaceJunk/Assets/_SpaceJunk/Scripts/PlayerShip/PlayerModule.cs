using Photon.Pun;
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
    public AudioSource myStructureSound;

    public float structureHP = 50f;
    public float structureMaxHP = 100f;
    public float energyCapacity = 100f;

    private PhotonView myPV;

    [PunRPC]
    public void playModuleSound(AudioClip theClip)
    {
        myStructureSound.clip = theClip;
        myStructureSound.Play();
    }

    public void TestHullStrain(float CalculateHullStrain)
    {
        if ((Random.Range(1, 6) == 1)) structureHP++;
        if ( Random.Range(0f,1000f) <= CalculateHullStrain )
        { // percentage chance each hull part takes damage
            structureHP -= CalculateHullStrain * 0.1f;
            myPV.RPC("playModuleSound", RpcTarget.All, myShip.myGM.SoundBank.MetalStrainHigh[0];);

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
