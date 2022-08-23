using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfModules : MonoBehaviour
{
    public PlayerShip myShip;

    public GameObject ModuleListItemPrefab;

    public float moduleSpaceing = 0f;

    void MakeListOfModules()
    {
        foreach ( PlayerModule module in myShip.Modules)
        {
            Instantiate(ModuleListItemPrefab, transform.parent);
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
