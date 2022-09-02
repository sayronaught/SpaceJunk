using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneController : MonoBehaviour
{

    public GameObject MyThruster;
    public Transform myDrill;
    public Transform drillActive;
    public Transform drillInactive;
    public AudioSource myDrillSound;
    public PlayerDroneDrillHead myDrillHead;
    public AudioClip sfxDrillingLoop;
    public AudioClip sfxMiningLoop;

    public PlayerVrControls thisPlayer;
    public PlayerStation thisStation;

    public bool holdingRight;
    public Vector3 RightStartPos;
    public Quaternion RightStartRot;

    private PhotonView myPV;
    private Rigidbody myRB;

    private Vector3 movementCalc;
    private Quaternion rotationCalc;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody>();
    }

    private void moveFromRight()
    { // player is controlling from right controller
        movementCalc = RightStartPos - thisPlayer.rightController.transform.localPosition;
        myRB.AddRelativeForce(movementCalc*-100f);
        rotationCalc = RightStartRot * Quaternion.Inverse(thisPlayer.rightController.transform.localRotation);
        myRB.AddRelativeTorque(rotationCalc.x*-2f,rotationCalc.y * -2f, rotationCalc.z * -1f);
    }

    private void Mining()
    { // player is trying to mine
        if ( myDrillHead.AsteroidsInRange.Count > 0)
        { // is there any asteroid in range?
            if (myDrillSound.clip != sfxMiningLoop) myDrillSound.clip = sfxMiningLoop;
        } else { // no asteroid in range
            if (myDrillSound.clip != sfxDrillingLoop) myDrillSound.clip = sfxDrillingLoop;
        }
    }

    private void PlayerLeftDrone()
    { // player has moved on from the drone

        // move to bay, transfer inventory, THEN remove it.. for now, just remove it..
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!thisStation.thisPlayer) PlayerLeftDrone();
        if (thisPlayer)
        { // this Drone is controlled by player, otherwise ignore
            if (thisPlayer.playerRightGrab)
            {
                MyThruster.SetActive(true);
                if (holdingRight)
                { // we have a start pos
                    moveFromRight();
                } else { // we do not, this is the time get one
                    holdingRight = true;
                    RightStartPos = thisPlayer.rightController.transform.localPosition;
                    RightStartRot = thisPlayer.rightController.transform.localRotation;
                }
            } else {
                MyThruster.SetActive(false);
                holdingRight = false;
                RightStartPos = Vector3.zero;
                RightStartRot = Quaternion.identity;
            }
            if ( thisPlayer.playerRightTrigger || thisPlayer.playerLeftTrigger )
            {
                myDrill.transform.localPosition = Vector3.Slerp(myDrill.transform.localPosition, drillActive.transform.localPosition, Time.deltaTime * 0.25f);
                myDrillSound.volume = 0.2f;
                Mining();
            } else {
                myDrill.transform.localPosition = Vector3.Slerp(myDrill.transform.localPosition, drillInactive.transform.localPosition, Time.deltaTime * 0.25f);
                myDrillSound.volume = 0f;
            }
        }
    }
}
