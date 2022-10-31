using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipSpeed : MonoBehaviour
{

    private float updateTimer = 0f;

    private TMP_Text myTxt;

    private Rigidbody shipRB;

    private float convertSpeed;

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
            convertSpeed = shipRB.velocity.magnitude * 3.6f;
            myTxt.text = convertSpeed.ToString("F1").Replace(".", ",") + " km/h";
            updateTimer = 1f;
        }
        updateTimer -= Time.deltaTime;
    }
}
