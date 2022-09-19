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

    private float updateTimer = 1f;

    void MakeInventoryList()
    {
        if (myRect.transform.childCount > 0)
        {
            foreach (Transform child in myRect.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        myRect.sizeDelta = new Vector2(0, myShip.Inventory.Inventory.Count * spacing);
        if ( myShip.Inventory.Inventory.Count > 0)
        {
            for (int i  = 0; i < myShip.Inventory.Inventory.Count ; i++ )
            {
                if (myShip.Inventory.Inventory[i].amount > 0)
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
        updateTimer = 1f;
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
        if (updateTimer < 0f) MakeInventoryList();
        updateTimer -= Time.deltaTime;
    }
}
