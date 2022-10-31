using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstPersonController : MonoBehaviour
{
    public Transform camera;

    public float yRot = 0f;
    public float xRot = 0f;
    public void updateFirstPersonController()
    {
        if ( Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 )
        {
            yRot += Input.GetAxis("Mouse X") * 500f * Time.deltaTime;
            xRot += Input.GetAxis("Mouse Y") * -250f * Time.deltaTime;
            xRot = Mathf.Clamp(xRot, -90, 90);
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.position += transform.right * Input.GetAxis("Horizontal") *3f * Time.deltaTime;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.position += transform.forward * Input.GetAxis("Vertical") * 4f * Time.deltaTime;
        }
        transform.localRotation = Quaternion.Euler(0f, yRot, 0f);
        camera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
}
