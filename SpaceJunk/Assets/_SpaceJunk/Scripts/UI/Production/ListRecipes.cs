using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListRecipes : MonoBehaviour
{

    public PlayerShip myShip;

    public GameObject RecipeListItemPrefab;

    public float spacing = 55f;

    private RectTransform myRect;

    private float updateTimer = 1f;

    void MakeRecipeList()
    {
        if ( myRect.transform.childCount > 0)
        {
            foreach (Transform child in myRect.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        myRect.sizeDelta = new Vector2(0, myShip.Recipes.RecipeList.Count * spacing);
        if (myShip.Recipes.RecipeList.Count > 0)
        {
            for (int i = 0; i < myShip.Recipes.RecipeList.Count; i++)
            {
                var listItem = Instantiate(RecipeListItemPrefab, transform);
                listItem.transform.GetChild(0).GetComponent<RawImage>().texture = myShip.Recipes.RecipeList[i].recipeIcon;
                listItem.transform.GetChild(1).GetComponent<TMP_Text>().text = myShip.Recipes.RecipeList[i].recipeName;
                listItem.transform.GetChild(2).GetComponent<TMP_Text>().text = myShip.Recipes.RecipeList[i].recipeDescription;
                string usedUp = "Power: " + myShip.Recipes.RecipeList[i].energyCost;
                if (myShip.Recipes.RecipeList[i].usedup.Length > 0)
                {
                    for ( int ii = 0; ii < myShip.Recipes.RecipeList[i].usedup.Length; ii++ )
                    {
                        usedUp += "\n" + myShip.Recipes.RecipeList[i].usedup[ii].item.itemName + " : " + myShip.Recipes.RecipeList[i].usedup[ii].amount;
                    }
                }
                listItem.transform.GetChild(3).GetComponent<TMP_Text>().text = usedUp;
            }
        }
        updateTimer = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        myShip = GameObject.Find("PlayerShip").GetComponent<PlayerShip>();
        myRect = GetComponent<RectTransform>();
        MakeRecipeList();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateTimer < 0f) MakeRecipeList();
        updateTimer -= Time.deltaTime;
    }
}
