using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public PlayerVrControls playerVrControls;
    public List<PlayerModule> Modules;

    public float energy = 500f;
    public float energyMax = 1000f;

    public float structureHP = 500f;
    public float structureHPMax = 1000f;

    public float metal = 500f;
    public float metalMax = 1000f;

    public float metalFatigue = 0f;

    public int controlSpeedStage = 0;
    public Vector2 controlsYawPitch;
    public Vector2 controlsLeft;
    public Vector2 controlsRight;

    // speed particles
    public Transform SpeedParticles;
    private ParticleSystem SpeedParticlesFast;

    public SO_Item_Inventory Inventory;

    public gameManager myGM;

    // non hosts, needs to know hwere to move ship towards
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float updateTimer = 0f;
    private int updateTimerSkips = 0;


    private PhotonView myPV;
    private Rigidbody myRB;

    private float CalculateHullStrain;
    private float CalculateMass;
    private ParticleSystem.EmissionModule em;

    [PunRPC]
    public void sendCockpitControlSpeed(int changeSpeed)
    {
        controlSpeedStage = changeSpeed;
    }
    [PunRPC]
    public void sendCockpitControlStick(Vector2 stickInput)
    {
        controlsYawPitch = stickInput;
    }

    [PunRPC]
    public void updateShipFromHost(Vector3 targetPos,Quaternion targetRot, Vector3 velocity, Vector3 rotation)
    { // host sends position and movement to other ships
        targetPosition = targetPos;
        targetRotation = targetRot;
        myRB.velocity = velocity;
        myRB.angularVelocity = rotation;
    }

    [PunRPC]
    public void updateClientFromMasterUpdateTick(float en, float enMax, float hp, float hpMax)
    { // host sends tick info
        energy = en;
        energyMax = enMax;
        structureHP = hp;
        structureHPMax = hpMax;
    }

    [PunRPC]
    public void updateInventory(string newInv)
    { // host sends tick info
        Inventory = new SO_Item_Inventory();
        Inventory.addJSON(newInv);
    }

    public void addToInventory( string stuffToAdd )
    {
        Inventory.addJSON(stuffToAdd);
        myPV.RPC("updateInventory", RpcTarget.All, JsonUtility.ToJson(Inventory));
    }

    private void SpeedParticlesUpdate()
    {
        // slow = 0-10, hyper = 11-30, ludicrous = 31-200, 
        if (myRB.velocity.magnitude > 1)
            SpeedParticles.LookAt(transform.position + myRB.velocity);
        em = SpeedParticlesFast.emission;
        em.rateOverTime = Mathf.Clamp(myRB.velocity.magnitude-5f, 0f, 20f);
    }

    private void masterClientUpdateTick()
    { // master client ticks energy & HP
        updateTimerSkips++;
        if ( updateTimerSkips > 4 )
        { // current settings around each second
            structureHP = 0;
            structureHPMax = 0;
            energyMax = 0;
            CalculateHullStrain = myRB.velocity.magnitude * (myRB.angularVelocity.magnitude + 2f);
            foreach ( PlayerModule Module in Modules)
            {
                Module.TestHullStrain(CalculateHullStrain);
                structureHP += Module.structureHP;
                structureHPMax += Module.structureMaxHP;
                energyMax += Module.energyCapacity;
            }
            if (energy < energyMax) energy++;
            myPV.RPC("updateClientFromMasterUpdateTick", RpcTarget.All, energy, energyMax, structureHP, structureHPMax);
            updateTimerSkips = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody>();
        SpeedParticlesFast = SpeedParticles.GetChild(0).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //playerVrControls.SendLeftHaptics(0, 0.1f, 1f);
        //playerVrControls.SendRightHaptics(0, 0.1f, 1f);
        if (PhotonNetwork.IsMasterClient)
        { // host sends ship updates
            if ( updateTimer < 0 )
            {
                myPV.RPC("updateShipFromHost", RpcTarget.All, transform.position, transform.rotation,myRB.velocity,myRB.angularVelocity);
                masterClientUpdateTick();
                updateTimer = 0.2f;
            }
            updateTimer -= Time.deltaTime;
        } else { // non hosts, gently move there
            targetPosition += myRB.velocity;
            targetRotation *= Quaternion.Euler(myRB.angularVelocity);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime);
        }
        SpeedParticlesUpdate();

        // Mass
        CalculateMass = 0f;
        foreach ( PlayerModule module in Modules)
        {
            CalculateMass += module.myMass;
        }
        myRB.mass = CalculateMass + Inventory.getCombinedMass();
    }
}
