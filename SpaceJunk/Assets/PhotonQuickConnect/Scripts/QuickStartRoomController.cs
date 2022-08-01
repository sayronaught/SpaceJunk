using Photon.Pun;
using UnityEngine;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int MultiplayerSceneIndex;

    public override void OnEnable()
    {
        //base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        //base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        Debug.Log("Joined room");
        StartGame();
    }
    void StartGame() 
    { 
        if ( PhotonNetwork.IsMasterClient )
        {
            Debug.Log("Starting game");
            PhotonNetwork.LoadLevel(MultiplayerSceneIndex);
        }
    }
    
}
