using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCockpit : MonoBehaviour
{

    public bool isPlayerInSeat = false;
//    public bool isPlayerGameHost = false;
    public float checkPlayerSeatTimer = 1f;

    public PlayerShip myShip;
    public PlayerLever controlsSpeedLever;
    public PlayerJoystick controlsJoyStick;

    private PlayerStation myStation;
    private PhotonView myPV;

    private int speedLeverState;
    private int speedLeverOldState;
    private Vector2 joyStickInput;
    private bool holdingJoyStick = false;

    void checkPlayerSeat()
    {
        if (myStation.thisPlayer != null)
        {
            isPlayerInSeat = true;
        } else {
            isPlayerInSeat = false;
        }
        checkPlayerSeatTimer = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        myStation = GetComponentInChildren<PlayerStation>();
        myPV = myShip.gameObject.GetPhotonView();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkPlayerSeatTimer < 0f) checkPlayerSeat();
        checkPlayerSeatTimer -= Time.deltaTime;
        if ( isPlayerInSeat )
        {
            // getting the inputs
            speedLeverState = controlsSpeedLever.getLeverState();
            joyStickInput = controlsJoyStick.getJoyStickInput();
            holdingJoyStick = controlsJoyStick.isGrabbedRight;

            // setting the inputs
            myShip.controlSpeedStage = speedLeverState;
            myShip.controlsYawPitch = joyStickInput;

            if (speedLeverState != speedLeverOldState)
            { // if speed has been changed
                myPV.RPC("sendCockpitControlSpeed", RpcTarget.All, speedLeverState);
                speedLeverOldState = speedLeverState;
            }   
            if ((joyStickInput.x != 0f || joyStickInput.y != 0f) && holdingJoyStick )
            { // in there is joystick input, and player is actively holding the stick
                myPV.RPC("sendCockpitControlStick", RpcTarget.All, joyStickInput);
            }
        }
    }
}
