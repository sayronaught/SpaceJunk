using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockpit : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> leftHandDevices = new List<UnityEngine.XR.InputDevice>();

    bool leftTrigger = false;

    private PhotonView myPV;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void moveShip(Vector3 pos)
    {
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        if ( leftHandDevices.Count > 0)
        {
            leftHandDevices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTrigger);
        }
        if (leftTrigger)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime);
            Debug.Log("Trigger button is pressed.");
            myPV.RPC("moveShip", RpcTarget.All, transform.position);
        }
    }
}
