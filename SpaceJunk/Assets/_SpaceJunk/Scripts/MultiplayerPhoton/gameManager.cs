using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public class gameManager : MonoBehaviour
{

    public GameObject myXrRig;

    public void CreatePlayer()
    {
        Debug.Log("Creating player");
        var player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerShip"), Vector3.zero, Quaternion.identity);
        myXrRig.transform.SetParent(player.transform);
    }
    public async void CreateOtherPlayer()
    {
        while( true )
        {
            if ( GameObject.Find("PlayerShip(Clone)") != null )
            {
                myXrRig.transform.SetParent(GameObject.Find("PlayerShip(Clone)").transform);
                return;
            }
            await Task.Yield();
        }       
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
