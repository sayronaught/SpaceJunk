using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerTournament : MonoBehaviour
{
    public PlayerVrControls vrControls;
    public FirstPersonCharacterController firstPersonController;

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

        }
        else
        { // player have no headset
            //myXrRig.transform.position = refUISeat.transform.position;
            //myXrRig.transform.SetParent(refUISeat.transform);
            firstPersonController.updateFirstPersonController();
            Cursor.visible = false;
            //if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKey(KeyCode.Return)) myPV.RPC("allPlayersReboot", RpcTarget.All);
        }
    }
}
