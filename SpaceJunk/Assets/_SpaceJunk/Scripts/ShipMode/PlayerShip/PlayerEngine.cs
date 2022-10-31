using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    public PlayerShip myShip;

    public GameObject[] Thrusters;
    public AudioSource[] ThrusterAudio;

    [System.Serializable]
    public class thrustState
    {
        [Header("Thrust state")]
        [Tooltip("Is the visible thrust cones on or off?")]
        public bool onOrOff;
        [Tooltip("How big are the thrusters?")]
        public float Size;
        [Tooltip("How loud are the thrusters?")]
        public float Volume;
        [Tooltip("How do I play the sound?")]
        public float Pitch;
        [Tooltip("how much force does this engine apply?")]
        public float Force;
    }
    public thrustState[] thrustStates;

    public int lastThrustState = 1;
    [Tooltip("how much force does this engine apply when turning?")]
    public float navigationThrustForce = 5f;

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
        // visible thrusters on and off, and size
        foreach (GameObject Thruster in Thrusters)
        {
            // index out of bounds
            if ( myShip.controlSpeedStage > thrustStates.Length)
            {
                Debug.Log("Trying to set controlSpeedStage " + myShip.controlSpeedStage.ToString());
                return;
            }
            if ( thrustStates[myShip.controlSpeedStage].onOrOff)
            {
                Thruster.SetActive(true);
                Thruster.transform.localScale = new Vector3(thrustStates[myShip.controlSpeedStage].Size, thrustStates[myShip.controlSpeedStage].Size, thrustStates[myShip.controlSpeedStage].Size);
            } else {
                Thruster.SetActive(false);
            }
        }
        // visible thrusters audio volume
        foreach (AudioSource ass in ThrusterAudio)
        {
            ass.volume = thrustStates[myShip.controlSpeedStage].Volume;
            ass.pitch = thrustStates[myShip.controlSpeedStage].Pitch;
        }

        // how much push, forwards or backwards
        thrust = thrustStates[myShip.controlSpeedStage].Force;
        // actually push the ship
        if (thrust != 0f) myShipRB.AddRelativeForce(Vector3.forward * thrust * Time.deltaTime);

        // power brakes
        if (myShip.controlsBrake == 1)
        { // brakes on
            myShipRB.drag = myShipRB.angularDrag = 10f;
        } else { // brakes off
            myShipRB.drag = myShipRB.angularDrag = 1f;
        }

        // rotate the ship
        if ( myShip.controlsYawPitch.x != 0)
        {
            myShipRB.AddRelativeTorque(Vector3.up * myShip.controlsYawPitch.x * navigationThrustForce * Time.deltaTime);
        }
        if (myShip.controlsYawPitch.y != 0)
        {
            myShipRB.AddRelativeTorque(Vector3.left * myShip.controlsYawPitch.y * navigationThrustForce * -1f * Time.deltaTime);
        }
    }
}
