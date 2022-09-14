using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipMass : MonoBehaviour
{

    private float updateTimer = 0f;

    private TMP_Text myTxt;

    private Rigidbody shipRB;

    private float convertMass;

    // Start is called before the first frame update
    void Start()
    {
        myTxt = GetComponent<TMP_Text>();
        shipRB = GameObject.Find("PlayerShip").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateTimer < 0f)
        {
            convertMass = shipRB.mass * 0.001f;
            myTxt.text = convertMass.ToString("F1").Replace(".", ",") + " Tons";
            updateTimer = 1f;
        }
        updateTimer -= Time.deltaTime;
    }
}
