using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipSpeed : MonoBehaviour
{

    private float updateTimer = 0f;

    private TMP_Text myTxt;

    private Rigidbody shipRB;

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
            myTxt.text = shipRB.velocity.magnitude.ToString("F2")+ " m/s";
            updateTimer = 1f;
        }
        updateTimer -= Time.deltaTime;
    }
}
