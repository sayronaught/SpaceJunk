using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCharacterController : MonoBehaviour
{
    public float TurnRateHorizontal = 500f;
    public float ForceForward = 500000f;
    public float ForceSideway = 250000f;
    public float ForceJump = 50000f;
    public float DelayBetweenJumps = 0.5f;

    private float yRot = 0f;
    private float xRot = 0f;

    private Rigidbody myRB;
    private float JumpDelay = 0f;

    public Collider[] overlap;
    public bool grounded;

    private PlayerVrControls myVRController;

    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myVRController = GetComponent<PlayerVrControls>();
    }

    public void updateFirstPersonController()
    {
        JumpDelay -= Time.deltaTime;
        if (myVRController.playerLeftStick.x != 0)
        {
            transform.Rotate(transform.up, myVRController.playerLeftStick.x * TurnRateHorizontal * Time.deltaTime);
            //myRB.AddTorque(transform.up * TurnRateHorizontal * Time.deltaTime);
        }
        if (myVRController.playerRightStick.x != 0)
        {
            myRB.AddForce(transform.right * myVRController.playerRightStick.x * ForceSideway * Time.deltaTime);
        }
        if (myVRController.playerRightStick.y != 0)
        {
            myRB.AddForce(transform.forward * myVRController.playerRightStick.y * ForceForward * Time.deltaTime);
        }
        var overlap = Physics.OverlapBox(transform.position - (transform.up * 0.2f), Vector3.one * 0.15f, Quaternion.identity);
        if (overlap.Length > 0) grounded = true; else grounded = false;
        Debug.DrawLine(transform.position, transform.up * -1, Color.magenta);
        if (myVRController.playerRightStickClick && JumpDelay <= 0f)
        {

            if (grounded)
            {
                myRB.AddForce(transform.up * ForceJump);
                JumpDelay = DelayBetweenJumps;
            }
        }
    }
}
