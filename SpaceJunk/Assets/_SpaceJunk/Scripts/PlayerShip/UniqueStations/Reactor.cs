using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour
{
    public PlayerLever CrystalLever;

    private int crystalLeverState = 0;
    private bool alreadyPulled = false;
    private float timeToReset = 3f;

    private AudioSource myAS;

    // Start is called before the first frame update
    void Start()
    {
        myAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        crystalLeverState = CrystalLever.getLeverState();
        if ( crystalLeverState == 1)
        {
            if ( !alreadyPulled )
            { // first time, we add energy, eat crystal, play sound etcs
                myAS.Play();
                alreadyPulled = true;
            }
            timeToReset -= Time.deltaTime;
            if ( timeToReset <= 0f )
            { // time ran out, now we reset
                CrystalLever.changeLeverState(0);
                timeToReset = 3f;
            }
        }
    }
}
