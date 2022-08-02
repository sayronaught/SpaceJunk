using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class gameManager : MonoBehaviour
{

    public GameObject myXrRig;
    public GameObject playership;

    public void CreatePlayer()
    {
        Debug.Log("Creating player");
        var player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerShip"), Vector3.zero, Quaternion.identity);
        myXrRig.transform.SetParent(player.transform);
        playership = player;
    }
    public void CreateOtherPlayer()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
