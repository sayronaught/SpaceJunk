using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenu : MonoBehaviour
{

    public GameObject[] Pages;
    public AudioSource buttonSound;

    public void mainMenuPageOpen(int page)
    {
        int lastOpen = -1;

        //foreach (GameObject Page in Pages)
        for (int i = 0; i < Pages.Length; i++)
        {
            if (Pages[i].activeSelf) lastOpen = i;
            Pages[i].SetActive(false);
        }
        if ( lastOpen != page ) Pages[page].SetActive(true);
        buttonSound.Play();
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
