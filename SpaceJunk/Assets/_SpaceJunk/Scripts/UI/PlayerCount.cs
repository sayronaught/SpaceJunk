using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCount : MonoBehaviour
{

    private float updateTimer = 0f;

    private TMP_Text myTxt;

    // Start is called before the first frame update
    void Start()
    {
        myTxt = GetComponent<TMP_Text>();
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
