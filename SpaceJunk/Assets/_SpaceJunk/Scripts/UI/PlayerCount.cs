using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCount : MonoBehaviour
{

    private float updateTimer = 0f;

    private Text myTxt;

    // Start is called before the first frame update
    void Start()
    {
        myTxt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( updateTimer < 0f )
        {
            myTxt.text = PhotonNetwork.CountOfPlayers.ToString();
            updateTimer = 1f;
        }
        updateTimer -= Time.deltaTime;
    }
}
