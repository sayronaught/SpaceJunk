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
        public int amount;

        public Resource set(SO_Item newitem, int newamount)
        {
            var newResource = new Resource();
            item = item;
            amount = amount;
            return newResource;
        }
    }
    public List<Resource> Inventory;

    public SO_Item removeRandom()
    {
        if (Inventory.Count < 1) return null;
        int rand = Random.Range(0, Inventory.Count);
        SO_Item item = Inventory[rand].item;
        if (Inventory[rand].amount > 1) Inventory[rand].amount--;
        else Inventory.RemoveAt(rand);
        return item;
    }

    public void addItem( SO_Item item )
    { // this one can easily crash the whole thing
        if ( Inventory.Count> 0)
        {// if there is things on the list, we can do a foreach
            foreach (Resource res in Inventory)
            {
                if ( res.item == item )
                {
                    res.amount++;
                    return;
                }
            }
        }  // there is no list or items is not found, so we add it
        var newItem = new Resource();
        Inventory.Add(newItem.set(item, 1));
    }

}
