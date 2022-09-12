using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour
{
    public PlayerLever CrystalLever;

    private int crystalLeverState = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        crystalLeverState = CrystalLever.getLeverState();
    }
}
