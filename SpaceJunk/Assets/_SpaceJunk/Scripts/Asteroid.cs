using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Asteroid : MonoBehaviour
{

    public string AsteroidName = "Asteroid";
    public List<string> randomNameList;

    public float randomRotation = 250f;
    public float randomSpeed = 150f;
    public float randomScale = 5f;

    public float mineAble = 1f;
    public Vector3 scaleMineAble;
    public bool wasMinedSinceLast = false;

    public PlayerShip ThePlayersShip;

    public SO_Item_Inventory Inventory;

    private Rigidbody myRB;
    private PhotonView myPV;

    //private Vector3 targetPosition;

    private float updateTimer = 5f;

    // asteroid data recieved by anyone who is not host
    [PunRPC]
    public void updateAsteroid(Vector3 rbvelocity, Quaternion rbrotation,Vector3 newpos, Quaternion newrot, Vector3 newscale)
    {
        if (!myRB) getComps();
        myRB.velocity = rbvelocity;
        myRB.rotation = rbrotation;
        transform.position = newpos;
        transform.rotation = newrot;
        transform.localScale = newscale;
        if ( scaleMineAble == Vector3.zero ) scaleMineAble = newscale;
    }

    // asteroid mining data recieved by anyone who is not mining
    [PunRPC]
    public void wasMinedAsteroid(float newMineAble, string newInventory, string newName)
    {
        mineAble = newMineAble;
        Inventory = JsonUtility.FromJson<SO_Item_Inventory>(newInventory);
        AsteroidName = newName;
        transform.name = newName;
    }

    [PunRPC]
    public void RemoveAsteroid()
    {
        Destroy(gameObject, 1f);
    }

    private void getComps()
    {
        myRB = GetComponent<Rigidbody>();
        myPV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        getComps();
        myRB.AddTorque(new Vector3(Random.Range(-randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation), Random.Range(-randomRotation, randomRotation)));
        myRB.AddForce(new Vector3(Random.Range(-randomSpeed, randomSpeed), Random.Range(-randomSpeed, randomSpeed), Random.Range(-randomSpeed, randomSpeed)));
        transform.localScale = new Vector3(Random.Range(2f, randomScale), Random.Range(2f, randomScale), Random.Range(2f, randomScale));
        if (PhotonNetwork.IsMasterClient)
        {
            Inventory.RandomizeLoot();
            AsteroidName = randomNameList[Random.Range(0, randomNameList.Count)];
            AsteroidName = AsteroidName.Replace("INT", Random.Range(10,1000).ToString());
            myPV.RPC("wasMinedAsteroid", RpcTarget.All, mineAble, JsonUtility.ToJson(Inventory), AsteroidName);
        } else { // this is not the master
            transform.SetParent(GameObject.Find("Asteroids").transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && updateTimer < 0f)
        {
            if ( Vector3.Distance(ThePlayersShip.transform.position,transform.position) < 500f && mineAble > 0.1f)
            {// close enough
                myPV.RPC("updateAsteroid", RpcTarget.All, myRB.velocity, myRB.rotation, transform.position, transform.rotation, transform.localScale);
            } else { // too far away
                myPV.RPC("RemoveAsteroid", RpcTarget.All);
            }
            updateTimer = 5f;
        }
        if ( wasMinedSinceLast && updateTimer < 4.75f )
        {    
            myPV.RPC("updateAsteroid", RpcTarget.All, myRB.velocity, myRB.rotation, transform.position, transform.rotation, transform.localScale);
            myPV.RPC("wasMinedAsteroid", RpcTarget.All, mineAble, JsonUtility.ToJson(Inventory), AsteroidName);
            wasMinedSinceLast = false;
            updateTimer = 5f;
        }
        updateTimer -= Time.deltaTime;
    }
}
