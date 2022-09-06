using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListInventory : MonoBehaviour
{

    public PlayerShip myShip;

    public GameObject InventoryListItemPrefab;

    public float spacing = 55f;

    private RectTransform myRect;

    void MakeInventoryList()
    {
        myRect.sizeDelta = new Vector2(0, myShip.Inventory.Inventory.Count * spacing);
        if ( myShip.Inventory.Inventory.Count > 0)
        {
            for (int i  = 0; i < myShip.Inventory.Inventory.Count ; i++ )
            {
                var listItem = Instantiate(InventoryListItemPrefab, transform);
                listItem.transform.GetChild(0).GetComponent<RawImage>().texture = myShip.Inventory.Inventory[i].item.itemIcon;
                listItem.transform.GetChild(1).GetComponent<TMP_Text>().text = myShip.Inventory.Inventory[i].item.itemName;
                listItem.transform.GetChild(2).GetComponent<TMP_Text>().text = myShip.Inventory.Inventory[i].item.itemDescription;
                listItem.transform.GetChild(3).GetComponent<TMP_Text>().text = myShip.Inventory.Inventory[i].amount.ToString();
                listItem.transform.GetChild(4).GetComponent<TMP_Text>().text = (myShip.Inventory.Inventory[i].item.itemMass * myShip.Inventory.Inventory[i].amount).ToString() + " Kg";
            }
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
        myShip = GameObject.Find("PlayerShip").GetComponent<PlayerShip>();
        myRect = GetComponent<RectTransform>();
        MakeInventoryList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
