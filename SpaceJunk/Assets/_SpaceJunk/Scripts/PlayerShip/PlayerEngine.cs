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

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
