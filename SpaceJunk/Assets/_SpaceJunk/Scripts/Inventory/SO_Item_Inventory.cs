using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            item = newitem;
            amount = newamount;
            return newResource;
        }
    }
    public List<Resource> Inventory;

    private float massCalc;

    //constructor
    public SO_Item_Inventory()
    {
        Inventory = new List<Resource>();
    }

    public int countItem( string countName )
    {
        if (Inventory.Count > 0)
        {// if there is things on the list, we can do a foreach
            foreach (Resource res in Inventory)
            {
                if (res.item.itemName == countName)
                {
                    return res.amount;
                }
            }
        }
        return 0;
    }

    public SO_Item removeRandom()
    { // remove one random item and return it
        if (Inventory.Count < 1) return null;
        int rand = Random.Range(0, Inventory.Count);
        SO_Item tempitem = Inventory[rand].item;
        if (Inventory[rand].amount > 1) Inventory[rand].amount--;
        else Inventory.RemoveAt(rand);
        return tempitem;
    }

    public void addItem( SO_Item item, int num )
    { // this one can easily crash the whole thing
        if (item == null) return;
        if ( Inventory.Count> 0)
        {// if there is things on the list, we can do a foreach
            foreach (Resource res in Inventory)
            {
                if ( res.item == item )
                {
                    res.amount += num;
                    return;
                }
            }
        }  // there is no list or items is not found, so we add it
        var newItem = new Resource();
        newItem.set(item, num);
        Inventory.Add(newItem);
    }
    public void addItem(SO_Item item)
    { // just add one
        addItem(item, 1);
    }

    public void removeItem( SO_Item item, int num )
    {
        if (item == null) return;
        if (Inventory.Count > 0)
        {// if there is things on the list, we can do a foreach
            foreach (Resource res in Inventory)
            {
                if (res.item == item)
                {
                    res.amount -= num;
                    if (res.amount <= 0) Inventory.Remove(res);
                    return;
                }
            }
        }  // there is no list or items is not found, bugger!
        Debug.Log("Trying to remove item that isn't there: " + item.itemName);
    }

    public void removeItem(string removeName, int num)
    {
        if (Inventory.Count > 0)
        {// if there is things on the list, we can do a foreach
            foreach (Resource res in Inventory)
            {
                if (res.item.itemName == removeName)
                {
                    res.amount -= num;
                    if (res.amount <= 0) Inventory.Remove(res);
                    return;
                }
            }
        }  // there is no list or items is not found, bugger!
        Debug.Log("Trying to remove item that isn't there(string): " + removeName);
    }

    public void addJSON(string newItems)
    { // get a JSON string, and adds that to inventory
        SO_Item_Inventory temp = JsonUtility.FromJson<SO_Item_Inventory>(newItems);
        while ( temp.Inventory.Count > 0 )
        { // keep adding and removing until none are left
            addItem(temp.Inventory[0].item, temp.Inventory[0].amount);
            temp.Inventory.RemoveAt(0);
        }
    }

    public float getCombinedMass()
    {
        massCalc = 0f;
        if (Inventory.Count > 0)
        {
            foreach (Resource eachItem in Inventory)
            {
                massCalc += (float)eachItem.amount * eachItem.item.itemMass;
            }
        }
        return massCalc;
    }

    public bool checkInventoryForItem(SO_Item item, int amount)
    {
        if ( Inventory.Count > 0 )
        {
            foreach(Resource checkThis in Inventory)
            {
                if (item.itemName == checkThis.item.itemName && checkThis.amount >= amount) return true;
            }
        }
        return false;
    }

    public bool checkInventoryForRecipe(Resource[] ingredients)
    {
        if ( ingredients.Length > 0)
        {
            for (int i = 0; i < ingredients.Length; i++)
            {
                if (!checkInventoryForItem(ingredients[i].item,ingredients[i].amount)) return false;
            }
        }
        return true;
    }

}
