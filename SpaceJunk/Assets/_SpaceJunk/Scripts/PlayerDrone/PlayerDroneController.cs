using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneController : MonoBehaviour
{
    public float myMass = 200f; //200kg as standard

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
    public PlayerShip thisShip;

    public bool holdingRight;
    public Vector3 RightStartPos;
    public Quaternion RightStartRot;

    public SO_Item_Inventory Inventory;

    private PhotonView myPV;
    private Rigidbody myRB;


    // non hosts, needs to know hwere to move ship towards
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float updateTimer = 0f;
    private int updateTimerSkips = 0;

    private bool playerLeftTheDrone = false;
    private Vector3 movementCalc;
    private Quaternion rotationCalc;

    [PunRPC]
    public void updateDroneFromController(Vector3 targetPos, Quaternion targetRot, Vector3 velocity, Vector3 rotation)
    { // host sends position and movement to other ships
        targetPosition = targetPos;
        targetRotation = targetRot;
        myRB.velocity = velocity;
        myRB.angularVelocity = rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody>();
    }

    private void moveFromRight()
    { // player is controlling from right controller
        movementCalc = RightStartPos - thisPlayer.rightController.transform.localPosition;
        myRB.AddRelativeForce(movementCalc*-100f*myMass);
        rotationCalc = RightStartRot * Quaternion.Inverse(thisPlayer.rightController.transform.localRotation);
        myRB.AddRelativeTorque(rotationCalc.x*-1f*myMass,rotationCalc.y * -1f*myMass, rotationCalc.z * -0.5f*myMass);
    }

    private void Mining()
    { // player is trying to mine
        if ( myDrillHead.AsteroidsInRange.Count > 0)
        { // is there any asteroid in range?
            if (myDrillSound.clip != sfxMiningLoop) myDrillSound.clip = sfxMiningLoop;
            myDrillSound.volume = 0.25f;
            foreach (Asteroid ast in myDrillHead.AsteroidsInRange)
            {
                if ( ast.mineAble > 0f)
                {
                    ast.wasMinedSinceLast = true;
                    ast.mineAble -= Time.deltaTime * 0.5f;
                    ast.transform.localScale = ast.scaleMineAble*ast.mineAble;
                }
                if ( ast.mineAble < 0.9f && ast.Inventory.Inventory.Count > 0)
                { // Can be mined and there is things to mine
                    Inventory.addItem( ast.Inventory.removeRandom() );
                }
            }
        } else { // no asteroid in range
            if (myDrillSound.clip != sfxDrillingLoop) myDrillSound.clip = sfxDrillingLoop;
            myDrillSound.volume = 0.25f;
        }
    }


    // asteroid mining data recieved by anyone who is not mining
    [PunRPC]
    public void removeDrone()
    {
        Destroy(gameObject);
    }

    private void PlayerLeftDrone()
    { // player has moved on from the drone

        // move to bay, transfer inventory, THEN remove it.. for now, just remove it..
        if (playerLeftTheDrone) return;
        playerLeftTheDrone = true;
        thisShip.addToInventory(JsonUtility.ToJson(Inventory));
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
                
                Mining();
            } else {
                myDrill.transform.localPosition = Vector3.Slerp(myDrill.transform.localPosition, drillInactive.transform.localPosition, Time.deltaTime * 0.25f);
                myDrillSound.volume = 0f;
            }

            // update other clients
            if (updateTimer < 0)
            {
                myPV.RPC("updateDroneFromController", RpcTarget.All, transform.position, transform.rotation, myRB.velocity, myRB.angularVelocity);
                updateTimer = 0.2f;
            }
            updateTimer -= Time.deltaTime;

            // mass calculations
            myRB.mass = myMass + Inventory.getCombinedMass();
        } else { // this drone is controlled by another player

            targetPosition += myRB.velocity;
            targetRotation *= Quaternion.Euler(myRB.angularVelocity);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime);

        }
    }
}
