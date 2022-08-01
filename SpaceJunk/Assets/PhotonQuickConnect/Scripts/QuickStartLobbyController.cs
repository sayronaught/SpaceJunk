using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject QuickStartButton;
    [SerializeField]
    private GameObject QuickStartCancel;
    [SerializeField]
    private int roomSize;

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        PhotonNetwork.AutomaticallySyncScene=true;
        QuickStartButton.SetActive(true);
    }

    public void QuickStart()
    {
        QuickStartButton.SetActive(false);
        QuickStartCancel.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick Start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Failed to join a room");
        CreateRoom();
    }
    void CreateRoom()
    {
        Debug.Log("Creating room now");
        int RandomRoomNumber = Random.Range(0, 10000);
        RoomOptions RoomOp = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom("QuickRoom "+RandomRoomNumber,RoomOp);
        Debug.Log("Room " + RandomRoomNumber);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Failed creating room, trying again");
        CreateRoom();
    }
    
    public void QuickCancel()
    {
        QuickStartButton.SetActive(true);
        QuickStartCancel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        Debug.Log("Leaving Room");
    }
}
