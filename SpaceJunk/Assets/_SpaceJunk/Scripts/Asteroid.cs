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

    //private Vector3 targetPosition;

    private float updateTimer = 5f;

    // asteroid data recieved by anyone who is not host
    [PunRPC]
    public void updateAsteroid(Vector3 rbvelocity, Quaternion rbrotation,Vector3 newpos, Quaternion newrot, Vector3 newscale)
    {
        myRB.velocity = rbvelocity;
        myRB.rotation = rbrotation;
        transform.position = newpos;
        transform.rotation = newrot;
        transform.localScale = newscale;
        //currentPlayers = players;
    }

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myPV = GetComponent<PhotonView>();
        myRB.AddTorque(new Vector3(Random.Range(-randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation)));
        myRB.AddForce(new Vector3(Random.Range(-randomSpeed, randomSpeed), Random.Range(-randomSpeed, randomSpeed), Random.Range(-randomSpeed, randomSpeed)));
        transform.localScale = new Vector3(Random.Range(2f,randomScale), Random.Range(2f, randomScale), Random.Range(2f, randomScale));
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && updateTimer < 0f)
        {
            myPV.RPC("updateAsteroid", RpcTarget.All, myRB.velocity,myRB.rotation, transform.position, transform.rotation, transform.localScale);
            updateTimer = 5f;
        }
        updateTimer -= Time.deltaTime;
    }
}
