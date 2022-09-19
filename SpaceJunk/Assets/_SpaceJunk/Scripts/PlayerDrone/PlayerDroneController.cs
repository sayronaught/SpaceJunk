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

    private bool controlStick = false;

    [PunRPC]
    public void updateDroneFromController(Vector3 targetPos, Quaternion targetRot, Vector3 velocity, Vector3 rotation)
    { // host sends position and movement to other ships
        if (!myRB) Start();
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

    private void controlDrone()
    { // We are controlling the drone from THIS headset

        controlStick = false;

        // control via controller sticks
        if ( thisPlayer.playerLeftStick != Vector2.zero || thisPlayer.playerRightStick != Vector2.zero)
        {
            myRB.AddRelativeForce(Vector3.forward * 1000f *myMass* Time.deltaTime * thisPlayer.playerLeftStick.y);
            myRB.AddRelativeTorque(Vector3.forward * -2500f * Time.deltaTime * thisPlayer.playerRightStick.x);
            myRB.AddRelativeTorque(Vector3.up * 5000f * Time.deltaTime * thisPlayer.playerLeftStick.x);
            myRB.AddRelativeTorque(Vector3.left * 5000f * Time.deltaTime * thisPlayer.playerRightStick.y);
            controlStick = true;
        }

        // control via right grab
        if (thisPlayer.playerRightGrab && !controlStick)
        { // Player grabs right Grabbutton and then moved that hand
            MyThruster.SetActive(true);
            if (holdingRight)
            { // we have a start pos
                moveUsingRightGrabButton();
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

        // Mining
        if (thisPlayer.playerRightTrigger || thisPlayer.playerLeftTrigger)
        {
            myDrill.transform.localPosition = Vector3.Slerp(myDrill.transform.localPosition, drillActive.transform.localPosition, Time.deltaTime * 0.25f);
            Mining();
        }
        else
        {
            myDrill.transform.localPosition = Vector3.Slerp(myDrill.transform.localPosition, drillInactive.transform.localPosition, Time.deltaTime * 0.25f);
            myDrillSound.volume = 0f;
        }
    }

    private void moveUsingRightGrabButton()
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
        thisShip.gameObject.GetPhotonView().RPC("addToInventory", RpcTarget.MasterClient, JsonUtility.ToJson(Inventory));
        myPV.RPC("removeDrone", RpcTarget.All);
        //Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!thisStation.thisPlayer) PlayerLeftDrone();
        if (thisPlayer)
        { // this Drone is controlled by player, otherwise ignore

            // mass calculations
            myRB.mass = myMass + Inventory.getCombinedMass();

            controlDrone();

            // update other clients
            if (updateTimer < 0)
            {
                myPV.RPC("updateDroneFromController", RpcTarget.All, transform.position, transform.rotation, myRB.velocity, myRB.angularVelocity);
                updateTimer = 0.2f;
            }
            updateTimer -= Time.deltaTime;

            // drone HUD
            thisPlayer.myHud.setHudDroneSwivel(transform.position,transform.forward, transform.rotation);

        } else { // this drone is controlled by another player

            targetPosition += myRB.velocity;
            targetRotation *= Quaternion.Euler(myRB.angularVelocity);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime);

        }
    }
}
