using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockpit : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> leftHandDevices = new List<UnityEngine.XR.InputDevice>();
    List<UnityEngine.XR.InputDevice> rightHandDevices = new List<UnityEngine.XR.InputDevice>();

    bool leftTrigger = false;
    Vector2 leftStick;
    Vector2 rightStick;

    private PhotonView myPV;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void moveShip(Vector3 pos,Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    void updateAllEntities()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
        if ( leftHandDevices.Count > 0)
        {
            leftHandDevices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTrigger);
            leftHandDevices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out leftStick);
        }
        if (rightHandDevices.Count > 0)
        {
            //leftHandDevices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTrigger);
            rightHandDevices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out rightStick);
        }
        if (leftTrigger)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime);
            //Debug.Log("Trigger button is pressed.");
            myPV.RPC("moveShip", RpcTarget.All,transform.position,transform.rotation);
        }
        if (leftStick.x != 0f)
        {
            transform.Rotate(transform.forward, leftStick.x * Time.deltaTime *-25f);
            myPV.RPC("moveShip", RpcTarget.All, transform.position, transform.rotation);
        }
        if (leftStick.y != 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + (transform.forward*leftStick.y), Time.deltaTime*10f);
            myPV.RPC("moveShip", RpcTarget.All, transform.position, transform.rotation);
        }
        if (rightStick.x != 0f)
        {
            transform.Rotate(transform.up, rightStick.x * Time.deltaTime * 25f);
            myPV.RPC("moveShip", RpcTarget.All, transform.position, transform.rotation);
        }
        if (rightStick.y != 0f)
        {
            transform.Rotate(transform.right, rightStick.y * Time.deltaTime * -25f);
            myPV.RPC("moveShip", RpcTarget.All, transform.position, transform.rotation);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            updateAllEntities();
        }
    }
}
