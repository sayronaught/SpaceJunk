using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SO_Item_Inventory
{
    [System.Serializable]
    public class Resource
    {
        public SO_Item item;
        public float amount;
    }
    public List<Resource> Inventory;


}
