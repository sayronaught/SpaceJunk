using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListOfModules : MonoBehaviour
{
    public PlayerShip myShip;

    public GameObject ModuleListItemPrefab;

    public float spacing = 55f;

    private RectTransform myRect;

    void MakeListOfModules()
    {
        myRect.sizeDelta = new Vector2(0, myShip.Modules.Count * spacing);
        foreach ( PlayerModule module in myShip.Modules)
        {
            var listItem = Instantiate(ModuleListItemPrefab, transform);
            listItem.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = module.moduleName;
            listItem.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = module.moduleDescription;
            listItem.transform.GetChild(1).GetComponent<ListOfModulesHpBar>().myMod = module;
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
