using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneController : MonoBehaviour
{

    public PlayerVrControls thisPlayer;

    public bool holdingRight;
    public Vector3 RightStartPos;
    public Vector3 RightStartRot;

    private PhotonView myPV;
    private Rigidbody myRB;

    private Vector3 movementCalc;
    private Vector3 rotationCalc;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody>();
    }

    private void moveFromRight()
    {
        movementCalc = RightStartPos - thisPlayer.rightController.transform.localPosition;
        myRB.AddForce(movementCalc*-100f);
        rotationCalc = RightStartRot - thisPlayer.rightController.transform.localRotation.eulerAngles;
        if (rotationCalc.x > 180f) rotationCalc.x -= 360f;
        if (rotationCalc.y > 180f) rotationCalc.y -= 360f;
        if (rotationCalc.z > 180f) rotationCalc.z -= 360f;
        myRB.AddRelativeTorque(rotationCalc*0.00005f);
    }

    // Update is called once per frame
    void Update()
    {
        if (thisPlayer)
        { // this Drone is controlled by player, otherwise ignore
            if (thisPlayer.playerRightGrab)
            {
                if (holdingRight)
                { // we have a start pos
                    moveFromRight();
                } else { // we do not, this is the time get one
                    holdingRight = true;
                    RightStartPos = thisPlayer.rightController.transform.localPosition;
                    RightStartRot = thisPlayer.rightController.transform.localRotation.eulerAngles;
                }
            } else {
                holdingRight = false;
                RightStartPos = Vector3.zero;
                RightStartRot = Vector3.zero;
            }
        }
    }
}
