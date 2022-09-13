using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public float structureHP = 1000f;
    public float structureHPMax = 1000f;

    public SO_Item_Inventory Inventory;
    public gameManager myGM;

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
    public float Radius = 3.0f;

    public float TurnRate = 0.25f;
    public float Velocity = 2.0f;

    [PunRPC]
    public void updateShipFromHost(Vector3 targetPos, Quaternion targetRot, Vector3 velocity, Vector3 rotation)
    { // host sends position and movement to other ships
        targetPosition = targetPos;
        targetRotation = targetRot;
        myRB.velocity = velocity;
        myRB.angularVelocity = rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myPV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
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
                //rigidBody.velocity = transform.forward * Velocity;

                //transform.rotation = Quaternion.Slerp(transform.rotation,
                //transform.LookAt(myGM.myShip.transform.position);

                //transform.Rotate(45f, 0f, 0f);
                myRB.AddRelativeForce(Vector3.forward * 50000000f * Time.deltaTime);
                myPV.RPC("updateShipFromHost", RpcTarget.All, transform.position, transform.rotation, myRB.velocity, myRB.angularVelocity);
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
    }
}
