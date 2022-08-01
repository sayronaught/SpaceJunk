using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class gameSetupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }
    void CreatePlayer()
    {
        Debug.Log("Creating player");
        var player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PhotonPlayer"),Vector3.zero,Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
