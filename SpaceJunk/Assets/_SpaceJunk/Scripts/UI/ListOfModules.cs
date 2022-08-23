using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfModules : MonoBehaviour
{
    public PlayerShip myShip;

    public GameObject ModuleListItemPrefab;

    void MakeListOfModules()
    {
        foreach ( PlayerModule module in myShip.Modules)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MakeListOfModules(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
