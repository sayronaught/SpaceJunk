using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{
    public Transform camera;

    public float TurnRateHorizontal = 500f;
    public float TurnRateVertical = 250f;
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

    void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }

    public void updateFirstPersonController()
    {
        JumpDelay -= Time.deltaTime;
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            yRot += Input.GetAxis("Mouse X") * TurnRateHorizontal * Time.deltaTime;
            xRot += Input.GetAxis("Mouse Y") * -TurnRateVertical * Time.deltaTime;
            xRot = Mathf.Clamp(xRot, -90, 90);
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            myRB.AddForce(transform.right * Input.GetAxis("Horizontal") * ForceSideway * Time.deltaTime);
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            myRB.AddForce(transform.forward * Input.GetAxis("Vertical") * ForceForward * Time.deltaTime);
        }
        var overlap = Physics.OverlapBox(transform.position-(transform.up * 0.2f), Vector3.one * 0.15f, Quaternion.identity);
        if (overlap.Length > 0) grounded = true; else grounded = false;
        Debug.DrawLine(transform.position, transform.up*-1, Color.magenta);
        if ( Input.GetAxis("Jump") > 0 && JumpDelay <= 0f)
        {
            
            if ( grounded)
            {
                myRB.AddForce(transform.up * ForceJump);
                JumpDelay = DelayBetweenJumps;
            } 
        }
        transform.localRotation = Quaternion.Euler(0f, yRot, 0f);
        camera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
}
