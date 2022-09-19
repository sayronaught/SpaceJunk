using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerTurret : MonoBehaviour
{
    public Transform turretSwivel;
    public Transform aimTransform;
    public Transform[] barrelEnds;
    private int currentBarrel = 0;
    private float shootDelay = 0f;
    public GameObject AmmoPrefab;
    public string AmmoPrefabName = "Laser1";
    public float energyCost = 50f;
    public AudioClip sfxShoot;
    public AudioClip sfxMisfire;

    public float maxDegreesPerSecond = 5f;

    public PlayerShip myShip;

    private PlayerStation MyStation;
    private AudioSource myAS;
    private PhotonView myPV;
    private float updateTimer = 0f;

    private Quaternion aim;

    [PunRPC]
    public void updateTurret(Quaternion swivel)
    {
        if (MyStation.thisPlayer == null)
            turretSwivel.rotation = swivel;
    }

    [PunRPC]
    private void fireTheTurret(Quaternion swivel)
    {
        if (myShip.energy >= energyCost)
        {
            myShip.energy -= energyCost;
            turretSwivel.rotation = swivel;
            foreach (Transform barrel in barrelEnds)
            {
                var shot = Instantiate(AmmoPrefab, barrelEnds[currentBarrel].position, barrelEnds[currentBarrel].rotation);
                //var shot = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", AmmoPrefabName), barrel.position, barrel.rotation);
            }
            myAS.clip = sfxShoot;
            myAS.Play();
        }
        else
        {
            myAS.clip = sfxMisfire;
            myAS.Play();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MyStation = GetComponentInChildren<PlayerStation>();
        myAS = GetComponent<AudioSource>();
        myPV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( MyStation.thisPlayer != null)
        { // current player is seated in this turret right now
            aimTransform.rotation = Quaternion.Lerp(MyStation.thisPlayer.leftController.transform.rotation, MyStation.thisPlayer.rightController.transform.rotation, 0.5f);
            aimTransform.Rotate(Vector3.left, -90);
            turretSwivel.rotation = Quaternion.Lerp(turretSwivel.rotation, aimTransform.rotation, maxDegreesPerSecond * Time.deltaTime);
            //turretSwivel.rotation = Quaternion.Lerp(MyStation.thisPlayer.leftController.transform.rotation, MyStation.thisPlayer.rightController.transform.rotation, 0.5f);
            //turretSwivel.Rotate(Vector3.left, -90);
            if ( updateTimer < 0f )
            {
                myPV.RPC("updateTurret", RpcTarget.All, turretSwivel.rotation);
                updateTimer = 0.2f;
            }
            updateTimer -= Time.deltaTime;
            shootDelay -= Time.deltaTime;
            if ( shootDelay < 0f && (MyStation.thisPlayer.playerLeftTrigger || MyStation.thisPlayer.playerRightTrigger))
            {
                MyStation.thisPlayer.SendRightHaptics(0,.25f, 0.25f);
                MyStation.thisPlayer.SendLeftHaptics(0,.25f, 0.25f);
                myPV.RPC("fireTheTurret", RpcTarget.All, turretSwivel.rotation);
                shootDelay = 0.5f;
            }

            // turret HUD
            myShip.myGM.vrControls.myHud.setHudTurretSwivel(turretSwivel.position, turretSwivel.forward, turretSwivel.rotation);
        }
    }
}
