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
        leverStick.transform.Rotate(leverStates[currentLeverPosition].Angle, 0f, 0f);
        display.text = leverStates[currentLeverPosition].text;
        display.color = leverStates[currentLeverPosition].color;
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
        if ( isGrabbedLeft && !)
/*
if(hitToTest.collider.bounds.Contains(telePosition))
{
   print("point is inside collider");
}
*/
    }
}
