using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    private Rigidbody myRB;
    private PhotonView myPV;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myPV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( myPV.IsMine )
        {
            if (Input.GetAxis("Horizontal") != 0f)
            {
                myRB.AddForce(new Vector3(Input.GetAxis("Horizontal") * 10f, 0f, 0f));
            }
            if (Input.GetAxis("Vertical") != 0f)
            {
                myRB.AddForce(new Vector3(0f, 0f, Input.GetAxis("Vertical") * 10f));
            }
        }
    }
}
