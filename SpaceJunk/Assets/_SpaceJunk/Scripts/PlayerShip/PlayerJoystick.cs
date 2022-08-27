using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoystick : MonoBehaviour
{
    public Transform joyStick;
    public BoxCollider JoystickZone;

    public PlayerStation myStation;

    public bool isGrabbedLeft = false;
    public bool isGrabbedRight = false;

    public Vector2 StickInput;

    public Vector2 getJoyStickInput()
    {
        if (!myStation.thisPlayer)
        {
            isGrabbedLeft = false;
            isGrabbedRight = false;
        }
        if (isGrabbedRight && !myStation.thisPlayer.playerRightGrab) isGrabbedRight = false;
        if (!isGrabbedRight && myStation.thisPlayer.playerRightGrab)
        {// only do this check when both are true, since this takes come CPU time
            if (JoystickZone.bounds.Contains(myStation.thisPlayer.rightController.transform.position))
                isGrabbedRight = true;
        }
        if ( isGrabbedRight)
        {
            joyStick.transform.LookAt(myStation.thisPlayer.rightController.transform.position, transform.up);
            StickInput = new Vector2(joyStick.transform.localRotation.eulerAngles.y,joyStick.transform.localRotation.eulerAngles.x);
            if (StickInput.x > 180f) StickInput.x -= 360f;
            if (StickInput.y > 180f) StickInput.y -= 360f;
            if (!JoystickZone.bounds.Contains(myStation.thisPlayer.rightController.transform.position))
                isGrabbedRight = false;
        } else {
            joyStick.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            StickInput = Vector2.zero;
        }
        
        return StickInput;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
