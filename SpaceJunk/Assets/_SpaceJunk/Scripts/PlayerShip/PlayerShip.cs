using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public List<PlayerModule> Modules;

    public float energy = 500f;
    public float energyMax = 1000f;

    public float hp = 500f;
    public float hpMax = 1000f;

    public float metal = 500f;
    public float metalMax = 1000f;

    public float metalFatigue = 0f;

    public int controlSpeedStage = 0;
    public Vector2 controlsLeft;
    public Vector2 controlsRight;

    [PunRPC]
    public void sendCockpitControlSpeed(int changeSpeed)
    {
        controlSpeedStage = changeSpeed;
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
