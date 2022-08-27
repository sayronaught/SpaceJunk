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

    public float hp = 500f;
    public float hpMax = 1000f;

    public float metal = 500f;
    public float metalMax = 1000f;

    public float metalFatigue = 0f;

    public int controlSpeedStage = 0;
    public Vector2 controlsYawPitch;
    public Vector2 controlsLeft;
    public Vector2 controlsRight;

    // non hosts, needs to know hwere to move ship towards
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float updateTimer = 0f;
    private PhotonView myPV;
    private Rigidbody myRB;

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

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody>();
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
                updateTimer = 0.2f;
            }
            updateTimer -= Time.deltaTime;
        } else { // non hosts, gently move there
            targetPosition += myRB.velocity;
            targetRotation *= Quaternion.Euler(myRB.angularVelocity);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime);
        }
    }
}
