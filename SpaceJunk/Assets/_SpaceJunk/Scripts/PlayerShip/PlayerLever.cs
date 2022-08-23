using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLever : MonoBehaviour
{
    public Transform leverStick;
    public BoxCollider leverUp;
    public BoxCollider leverGrab;
    public BoxCollider leverDown;
    public TMP_Text display;

    public PlayerStation myStation;

    [System.Serializable]
    public class leverState
    {
        [Header("Lever State")]
        public float Angle;
        [Tooltip("What will be written in the display?")]
        public string text;
        public Color color;
    }
    public leverState[] leverStates;
    public int currentLeverPosition;

    private bool isGrabbedLeft = false;
    private bool isGrabbedRight = false;

    private AudioSource myAS;

    void setLever()
    {
        leverStick.transform.localRotation = Quaternion.Euler(leverStates[currentLeverPosition].Angle, 0f, 0f);
        display.text = leverStates[currentLeverPosition].text;
        display.color = leverStates[currentLeverPosition].color;
    }
    public void changeLeverState(int newState)
    { // change lever state, from player pulling it, or from engine failure or such..
        currentLeverPosition = newState;
        setLever();
        isGrabbedLeft = false;
        isGrabbedRight = false;
        myAS.Play();
    }

    public int getLeverState()
    {
        if ( !myStation.thisPlayer)
        {
            isGrabbedRight = false;
            isGrabbedLeft = false;
            return currentLeverPosition;
        }
        if (isGrabbedLeft && !myStation.thisPlayer.playerLeftGrab) isGrabbedLeft = false;
        if ( !isGrabbedLeft && myStation.thisPlayer.playerLeftGrab)
        {// only do this check when both are true, since this takes come CPU time
            if (leverGrab.bounds.Contains(myStation.thisPlayer.leftController.transform.position)) isGrabbedLeft = true;
        }
        if ( isGrabbedLeft )
        { // they have grabbed it, lets check if they move it
            if (leverUp.bounds.Contains(myStation.thisPlayer.leftController.transform.position))
            {
                if (currentLeverPosition < leverStates.Length) changeLeverState(currentLeverPosition + 1);
      
            }
            if (leverDown.bounds.Contains(myStation.thisPlayer.leftController.transform.position))
            {
                if (currentLeverPosition > 0) changeLeverState(currentLeverPosition - 1);
            }
        }
        return currentLeverPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        myAS = GetComponent<AudioSource>();
        setLever();        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
