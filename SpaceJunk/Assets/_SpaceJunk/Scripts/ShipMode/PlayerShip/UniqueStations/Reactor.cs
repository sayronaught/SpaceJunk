using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Reactor : MonoBehaviour
{
    public PlayerModule myModule;
    public PlayerLever CrystalLever;
    public TMP_Text cystalText;

    private int crystalLeverState = 0;
    private bool alreadyPulled = false;
    private float timeToReset = 3f;

    private AudioSource myAS;
    private PhotonView myPV;

    int crystalCount = 0;

    [PunRPC]
    public void playReactorSound()
    {
        myAS.Play();
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
        crystalCount = myModule.myShip.Inventory.countItem("Energy Crystal");
        cystalText.text = "Energy Crystals\n"+crystalCount.ToString();
        crystalLeverState = CrystalLever.getLeverState();
        if ( crystalLeverState == 1)
        {
            if ( !alreadyPulled )
            { // first time, we add energy, eat crystal, play sound etcs
                if ( crystalCount > 0)
                {
                    myModule.myShip.gameObject.GetPhotonView().RPC("useCrystal", RpcTarget.MasterClient);
                    myPV.RPC("playReactorSound",RpcTarget.All);
                }
                alreadyPulled = true;
            }
            timeToReset -= Time.deltaTime;
            if ( timeToReset <= 0f )
            { // time ran out, now we reset
                CrystalLever.changeLeverState(0);
                timeToReset = 3f;
                alreadyPulled=false;
            }
        }
    }
}
