using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneController : MonoBehaviour
{

    public PlayerVrControls thisPlayer;

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
    {
        movementCalc = RightStartPos - thisPlayer.rightController.transform.localPosition;
        myRB.AddRelativeForce(movementCalc*-100f);
        rotationCalc = RightStartRot * Quaternion.Inverse(thisPlayer.rightController.transform.localRotation);
        myRB.AddRelativeTorque(rotationCalc.x*-2f,rotationCalc.y * -2f, rotationCalc.z * -1f);
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
                    RightStartRot = thisPlayer.rightController.transform.localRotation;
                }
            } else {
                holdingRight = false;
                RightStartPos = Vector3.zero;
                RightStartRot = Quaternion.identity;
            }
        }
    }
}
