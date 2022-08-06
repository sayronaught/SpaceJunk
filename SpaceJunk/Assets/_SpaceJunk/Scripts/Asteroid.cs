using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float randomRotation = 500f;
    public float randomSpeed = 150f;
    private Rigidbody myRB;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myRB.AddTorque(new Vector3(Random.Range(-randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation)));
        myRB.AddForce(new Vector3(Random.Range(-randomSpeed, randomSpeed), Random.Range(-randomSpeed, randomSpeed), Random.Range(-randomSpeed, randomSpeed)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
