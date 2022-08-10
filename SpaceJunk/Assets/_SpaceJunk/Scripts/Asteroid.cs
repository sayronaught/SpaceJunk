using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float randomRotation = 500f;
    public float randomSpeed = 150f;
    public float randomScale = 5f;
    private Rigidbody myRB;
    private PhotonView myPV;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myPV = GetComponent<PhotonView>();
        myRB.AddTorque(new Vector3(Random.Range(-randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation)));
        myRB.AddForce(new Vector3(Random.Range(-randomSpeed, randomSpeed), Random.Range(-randomSpeed, randomSpeed), Random.Range(-randomSpeed, randomSpeed)));
        transform.localScale = new Vector3(Random.Range(1,randomScale), Random.Range(1, randomScale), Random.Range(1, randomScale));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
