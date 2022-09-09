using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class ListRecipes : MonoBehaviour
{

    public PlayerShip myShip;

    public GameObject RecipeListItemPrefab;

    public float spacing = 55f;

    private RectTransform myRect;

    private float updateTimer = 1f;

    private SO_Recipe_List recipeFilter;

    void clearTransformChildren(Transform needToClear)
    {
        if (needToClear.childCount > 0)
        {
            foreach (Transform child in needToClear)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    void filterRecipies()
    {
        recipeFilter = new SO_Recipe_List();
        if (myShip.Recipes.RecipeList.Count > 0)
        {
            for (int i = 0; i < myShip.Recipes.RecipeList.Count; i++)
            {
                if ( myShip.energy >= myShip.Recipes.RecipeList[i].energyCost)
                {
                    if ( myShip.Inventory.checkInventoryForRecipe(myShip.Recipes.RecipeList[i].required) )
                    {
                        recipeFilter.RecipeList.Add(myShip.Recipes.RecipeList[i]);
                    }
                }
            }
        }
    }

    void MakeRecipeList()
    {
        clearTransformChildren(myRect.transform);
        filterRecipies();
        myRect.sizeDelta = new Vector2(0, recipeFilter.RecipeList.Count * spacing);
        if (myShip.Recipes.RecipeList.Count > 0)
        {
            for (int i = 0; i < recipeFilter.RecipeList.Count; i++)
            {
                var listItem = Instantiate(RecipeListItemPrefab, transform);
                listItem.transform.GetChild(0).GetComponent<RawImage>().texture = recipeFilter.RecipeList[i].recipeIcon;
                listItem.transform.GetChild(1).GetComponent<TMP_Text>().text = recipeFilter.RecipeList[i].recipeName;
                listItem.transform.GetChild(2).GetComponent<TMP_Text>().text = recipeFilter.RecipeList[i].recipeDescription;
                int x = i; // weird bug, it delegates all as the last number unless you do this crappy hack
                var button = listItem.transform.GetChild(4).GetComponent<Button>();
                button.onClick.AddListener(delegate { CraftButton(x,button); });
                string usedUp = "Power: " + recipeFilter.RecipeList[i].energyCost;
                if (recipeFilter.RecipeList[i].usedup.Length > 0)
                {
                    for ( int ii = 0; ii < recipeFilter.RecipeList[i].usedup.Length; ii++ )
                    {
                        usedUp += "\n" + recipeFilter.RecipeList[i].usedup[ii].item.itemName + " : " + recipeFilter.RecipeList[i].usedup[ii].amount;
                    }
                }
                listItem.transform.GetChild(3).GetComponent<TMP_Text>().text = usedUp;
            }
        }
        updateTimer = 1f;
    }

    public async void CraftButton(int i,Button button)
    {
        //button.interactable = false;
        myShip.CraftRecipe(i);
        await Task.Delay(0);
        //button.interactable = true;
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
