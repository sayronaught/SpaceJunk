using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    public PlayerShip myShip;

    public GameObject[] Thrusters;

    [System.Serializable]
    public class thrustState
    {
        [Header("Lever State")]
        public bool onOrOff;
        [Tooltip("What will be written in the display?")]
        public float Size;
    }
    public thrustState[] thrustStates;

    public int lastThrustState = 1;

    private Rigidbody myShipRB;
    private float thrust = 0f;

    // Start is called before the first frame update
    void Start()
    {
        myShipRB = myShip.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject Thruster in Thrusters)
        {
            if ( thrustStates[myShip.controlSpeedStage].onOrOff)
            {
                Thruster.SetActive(true);
                Thruster.transform.localScale = new Vector3(thrustStates[myShip.controlSpeedStage].Size, thrustStates[myShip.controlSpeedStage].Size, thrustStates[myShip.controlSpeedStage].Size);
            } else {
                Thruster.SetActive(false);
            }
        }
        switch (myShip.controlSpeedStage)
        {
            case 0: thrust = -500f; break;
            case 1: thrust = 500f; break;
            case 2: thrust = 1000f; break;
            case 3: thrust = 2500f; break;
            case 4: thrust = 15000f; break;
            default:
                thrust = 0f; break;
        }
        if (thrust != 0f) myShipRB.AddRelativeForce(Vector3.forward * thrust * Time.deltaTime);
        if ( myShip.controlsYawPitch.x != 0)
        {
            myShipRB.AddRelativeTorque(Vector3.up * myShip.controlsYawPitch.x * 500f * Time.deltaTime);
        }
        if (myShip.controlsYawPitch.y != 0)
        {
            myShipRB.AddRelativeTorque(Vector3.left * myShip.controlsYawPitch.y * 500f * Time.deltaTime);
        }
    }
}
