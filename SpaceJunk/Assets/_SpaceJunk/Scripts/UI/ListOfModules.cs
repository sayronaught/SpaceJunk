using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ListOfModules : MonoBehaviour
{
    public PlayerShip myShip;

    public GameObject ModuleListItemPrefab;

    public float moduleSpaceing = 0f;

    void MakeListOfModules()
    {
        int pos = -50;
        foreach ( PlayerModule module in myShip.Modules)
        {
            var listItem = Instantiate(ModuleListItemPrefab, transform);
            listItem.transform.localPosition = new Vector3(0,pos,0);
            pos -= 50;
            listItem.transform.GetChild(0).GetComponent<TMP_Text>().text = module.moduleName;
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
