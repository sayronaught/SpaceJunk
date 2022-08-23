using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenu : MonoBehaviour
{

    public GameObject[] Pages;


    public void mainMenuPageOpen(int page)
    {
        foreach (GameObject Page in Pages)
        {
            Page.SetActive(false);
        }
        Pages[page].SetActive(true);
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
