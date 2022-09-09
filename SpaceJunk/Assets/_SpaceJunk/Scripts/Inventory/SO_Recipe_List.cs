using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SO_Recipe_List
{

    public List<SO_Recipe> RecipeList;


    //constructor
    public SO_Recipe_List()
    {
        RecipeList = new List<SO_Recipe>();
    }
}
