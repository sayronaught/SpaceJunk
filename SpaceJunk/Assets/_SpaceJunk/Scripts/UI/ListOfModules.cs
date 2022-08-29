using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListOfModules : MonoBehaviour
{
    public PlayerShip myShip;

    public GameObject ModuleListItemPrefab;

    public int modulePadding = 20;
    public int moduleSpaceing = 50;

    private RectTransform myRect;

    void MakeListOfModules()
    {
        myRect.sizeDelta = new Vector2(0, moduleSpaceing*myShip.Modules.Count+(modulePadding*2));
        int pos = -(moduleSpaceing + modulePadding);
        foreach ( PlayerModule module in myShip.Modules)
        {
            var listItem = Instantiate(ModuleListItemPrefab, transform);
            listItem.transform.localPosition = new Vector3(0,pos,0);
            pos -= moduleSpaceing;
            listItem.transform.GetChild(0).GetComponent<TMP_Text>().text = module.moduleName;
            listItem.transform.GetChild(1).GetComponent<TMP_Text>().text = module.moduleDescription;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
        MakeListOfModules(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
