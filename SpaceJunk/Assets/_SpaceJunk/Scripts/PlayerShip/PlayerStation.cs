using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStation : MonoBehaviour
{
    public int maxPlayers = 1;
    public int currentPlayers = 0;

    public PlayerVrControls thisPlayer;

    public List<GameObject> playerActiveObjects;

    public bool DroneStation = false;
    public string DronePrefabName;

    private PhotonView myPV;

    // update this number on other machines in-game
    [PunRPC]
    public void updatePlayerCount(int players)
    {
        currentPlayers = players;
    }

    // puts the current player in this seat
    public void addCurrentPlayer(PlayerVrControls playerControl)
    {
        thisPlayer = playerControl;
        currentPlayers++;
        myPV.RPC("updatePlayerCount", RpcTarget.All, currentPlayers);
        if (playerActiveObjects.Count > 0)
        {
            foreach (GameObject playerActiveObjects in playerActiveObjects)
            {
                playerActiveObjects.gameObject.SetActive(true);
            }
        }
    }

    // removes the current player from this seat
    public void removeCurrentPlayer()
    {
        thisPlayer = null;
        currentPlayers--;
        myPV.RPC("updatePlayerCount", RpcTarget.All, currentPlayers);
        if (playerActiveObjects.Count > 0)
        {
            foreach (GameObject playerActiveObjects in playerActiveObjects)
            {
                playerActiveObjects.gameObject.SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        if (playerActiveObjects.Count > 0)
        {
            foreach (GameObject playerActiveObjects in playerActiveObjects)
            {
                playerActiveObjects.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
