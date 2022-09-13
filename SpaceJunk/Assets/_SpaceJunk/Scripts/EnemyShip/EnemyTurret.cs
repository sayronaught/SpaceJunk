using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemyTurret : MonoBehaviour
{
    public Transform turretSwivel;
    public Transform[] barrelEnds;
    private float shootDelay = 0f;
    public string AmmoPrefabName = "Laser1";
    public AudioClip sfxShoot;
    public AudioClip sfxMisfire;
    public float badAim = 5f;

    public EnemyShip myShip;
    private AudioSource myAS;
    private PhotonView myPV;
    private float updateTimer = 0f;

    private Quaternion aim;

    [PunRPC]
    private void fireTheTurret()
    {
        foreach (Transform barrel in barrelEnds)
        {
            var shot = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", AmmoPrefabName), barrel.position, barrel.rotation);
        }
        myAS.clip = sfxShoot;
        myAS.Play();
    }

    [PunRPC]
    public void updateTurret(Quaternion swivel)
    {
            turretSwivel.rotation = swivel;
    }

    // Start is called before the first frame update
    void Start()
    {
        myAS = GetComponent<AudioSource>();
        myPV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            turretSwivel.LookAt(myShip.myGM.myShip.transform.position);
            turretSwivel.Rotate(new Vector3(Random.Range(-badAim,badAim), Random.Range(-badAim, badAim), Random.Range(-badAim, badAim)));
            if (updateTimer < 0f)
            {
                myPV.RPC("updateTurret", RpcTarget.All, turretSwivel.rotation);
                updateTimer = 0.2f;
            }
            updateTimer -= Time.deltaTime;
            shootDelay -= Time.deltaTime;
            if (shootDelay < 0f && Vector3.Distance(myShip.myGM.myShip.transform.position,transform.position) < 150f)
            {
                myPV.RPC("fireTheTurret", RpcTarget.All);
                shootDelay = 1f;
            }
        } else {
            turretSwivel.LookAt(myShip.myGM.myShip.transform.position);
        }
            
    }
}
