using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemyShip : MonoBehaviour
{

    public string EnemyName = "Enemy";
    public List<string> randomNameList;

    public float structureHP = 100f;
    public float structureHPMax = 100f;

    public SO_Item_Inventory Inventory;
    public gameManager myGM;

    private PlayerVRHudPersonal activePersonalHUD;

    // non hosts, needs to know hwere to move ship towards
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float updateTimer = 0f;
    private int updateTimerSkips = 0;

    private PhotonView myPV;
    private Rigidbody myRB;

    public enum OrbitalDirections
    {
        Prograde,
        Retrograde,
    }
    public OrbitalDirections OrbitalDirection = OrbitalDirections.Prograde;


    private float Azimuth = 0.0f;
    private float Zenith = 0.0f;
    public float Radius = 100.0f;

    public float TurnRate = 0.25f;
    public float Velocity = 2500000f;

    private float speedBoost = 0f;
    private float radiusBoost = 0f;

    [PunRPC]
    public void updateShipFromHost(Vector3 targetPos, Quaternion targetRot, Vector3 velocity, Vector3 rotation, float newHP, string newName)
    { // host sends position and movement to other ships
        targetPosition = targetPos;
        targetRotation = targetRot;
        myRB.velocity = velocity;
        myRB.angularVelocity = rotation;
        structureHP = newHP;
        EnemyName = newName;
        transform.name = newName;
    }

    void enemyShipDestroyed()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var explo = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "BigExplosion"), transform.position, Quaternion.identity);
            var loot = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "EnemyWreck"), transform.position, Quaternion.identity);
            loot.GetComponent<Asteroid>().ThePlayersShip = myGM.myShip;
            loot.transform.SetParent(GameObject.Find("Asteroids").transform);
        }
        Destroy(gameObject);
    }

    void PersonalHUD()
    {
        // object ref
        if (!myGM.vrControls.playerHasHeadSet) return;
        if (activePersonalHUD)
        { // have one set up
            activePersonalHUD.timeToLive = 2f;
            activePersonalHUD.transform.LookAt(transform.position);
        } else { // need to set one up
            var hud = Instantiate(myGM.PersonalHUDPrefab, Camera.main.transform.position,Quaternion.identity,GameObject.Find("HUDs").transform);
            activePersonalHUD = hud.GetComponent<PlayerVRHudPersonal>();
            activePersonalHUD.setEnemyTarget(EnemyName);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerAmmo" && PhotonNetwork.IsMasterClient)
        {
            structureHP -= 15;
            myPV.RPC("updateShipFromHost", RpcTarget.All, transform.position, transform.rotation, myRB.velocity, myRB.angularVelocity, structureHP, EnemyName);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myPV = GetComponent<PhotonView>();
        speedBoost = Random.Range(-250000f, 250000f);
        radiusBoost = Random.Range(-25f, 25f);
        myGM = GameObject.Find("_gameManager").GetComponent<gameManager>();
        if (PhotonNetwork.IsMasterClient)
        {
            EnemyName = randomNameList[Random.Range(0,randomNameList.Count)];
        } else {
            transform.SetParent(GameObject.Find("Enemies").transform);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        PersonalHUD();
        if (PhotonNetwork.IsMasterClient)
        { // host sends ship updates
            if (updateTimer < 0)
            {
                Azimuth = Mathf.Atan2(transform.position.z, transform.position.x);

                if (OrbitalDirection == OrbitalDirections.Prograde)
                    Azimuth += (Mathf.PI / 2.0f);
                else
                    Azimuth -= (Mathf.PI / 2.0f);

                Zenith = Mathf.Acos(transform.position.y / Vector3.Distance(transform.position, Vector3.zero));

                Vector3 markerPosition = myGM.myShip.transform.position;
                markerPosition.x = Radius * Mathf.Cos(Azimuth) * Mathf.Sin(Zenith);
                markerPosition.z = Radius * Mathf.Sin(Azimuth) * Mathf.Sin(Zenith);
                markerPosition.y = Radius * Mathf.Cos(Zenith);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-1 * (transform.position - markerPosition).normalized), Time.deltaTime * TurnRate);
                myRB.AddRelativeForce(Vector3.forward * Velocity * Time.deltaTime);
                myPV.RPC("updateShipFromHost", RpcTarget.All, transform.position, transform.rotation, myRB.velocity, myRB.angularVelocity, structureHP, EnemyName);
                updateTimer = 0.2f;
            }
            updateTimer -= Time.deltaTime;
        }
        else
        { // non hosts, gently move there
            targetPosition += myRB.velocity;
            targetRotation *= Quaternion.Euler(myRB.angularVelocity);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime);
        }
        if (structureHP < 0f) enemyShipDestroyed();
    }
}
