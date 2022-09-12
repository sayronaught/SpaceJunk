using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    public Transform turretSwivel;
    public Transform[] barrelEnds;
    private float shootDelay = 0f;
    public string AmmoPrefabName = "Laser1";
    public AudioClip sfxShoot;
    public AudioClip sfxMisfire;

    public EnemyShip myShip;
    private AudioSource myAS;
    private PhotonView myPV;
    private float updateTimer = 0f;

    private Quaternion aim;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
