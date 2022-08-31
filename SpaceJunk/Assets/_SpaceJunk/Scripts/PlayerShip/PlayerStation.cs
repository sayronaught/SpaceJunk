using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStation : MonoBehaviour
{
    public int maxPlayers = 1;
    public int currentPlayers = 0;

    public PlayerVrControls thisPlayer;

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
    }

    // removes the current player from this seat
    public void removeCurrentPlayer()
    {
        thisPlayer = null;
        currentPlayers--;
        myPV.RPC("updatePlayerCount", RpcTarget.All, currentPlayers);
    }

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
