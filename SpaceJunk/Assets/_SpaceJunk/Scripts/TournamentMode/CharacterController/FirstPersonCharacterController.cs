using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{
    public Transform camera;

    public float TurnRateHorizontal = 500f;
    public float TurnRateVertical = 250f;

    private float yRot = 0f;
    private float xRot = 0f;

    public void updateFirstPersonController()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            yRot += Input.GetAxis("Mouse X") * TurnRateHorizontal * Time.deltaTime;
            xRot += Input.GetAxis("Mouse Y") * -TurnRateVertical * Time.deltaTime;
            xRot = Mathf.Clamp(xRot, -90, 90);
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.position += transform.right * Input.GetAxis("Horizontal") * 3f * Time.deltaTime;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.position += transform.forward * Input.GetAxis("Vertical") * 4f * Time.deltaTime;
        }
        transform.localRotation = Quaternion.Euler(0f, yRot, 0f);
        camera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
}
