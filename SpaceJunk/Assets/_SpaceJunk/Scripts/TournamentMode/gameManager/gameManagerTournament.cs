using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerTournament : MonoBehaviour
{
    public PlayerVrControls vrControls;
    public FirstPersonCharacterController firstPersonController;
    public VRCharacterController vrFirstPersonController;

    public GameObject myXrRig;

    private Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        //Physics.gravity = new Vector3(0f, -9.81f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (vrControls.playerHasHeadSet)
        {// player has headset
            vrFirstPersonController.updateFirstPersonController();
        }
        else
        { // player have no headset
            firstPersonController.updateFirstPersonController();
            Cursor.visible = false;
            //if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKey(KeyCode.Return)) myPV.RPC("allPlayersReboot", RpcTarget.All);
        }
    }
}
