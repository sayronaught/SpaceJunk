using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListNavigation : MonoBehaviour
{

    public PlayerShip myShip;

    public GameObject NavigationListItemPrefab;

    public float spacing = 55f;

    private RectTransform myRect;

    private float updateTimer = 1f;

    private Transform AsteroidContainer;
    private Asteroid asteroidScript;

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

    void makeNavigationList()
    {
        clearTransformChildren(myRect.transform);
        myRect.sizeDelta = new Vector2(0, AsteroidContainer.childCount * spacing);
        if ( AsteroidContainer.childCount > 0)
        {
            foreach(Transform asteroid in AsteroidContainer)
            {
                asteroidScript = asteroid.GetComponent<Asteroid>();
                var listItem = Instantiate(NavigationListItemPrefab, transform);

                // listItem.transform.GetChild(0).GetComponent<RawImage>().texture = recipeFilter.RecipeList[i].recipeIcon;
                listItem.transform.GetChild(1).GetComponent<TMP_Text>().text = asteroidScript.AsteroidName;
                listItem.transform.GetChild(2).GetComponent<TMP_Text>().text = Vector3.Distance(asteroidScript.ThePlayersShip.transform.position, asteroidScript.transform.position).ToString("F1") + " M";
                //string keepName = recipeFilter.RecipeList[i].recipeName;
                //var button = listItem.transform.GetChild(4).GetComponent<Button>();
                //button.onClick.AddListener(delegate { CraftButton(keepName); });
                string usedUp = "";
                if (asteroidScript.Inventory.Inventory.Count > 0)
                {
                    for (int ii = 0; ii < asteroidScript.Inventory.Inventory.Count; ii++)
                    {
                        usedUp += asteroidScript.Inventory.Inventory[ii].item.itemName + " : " + asteroidScript.Inventory.Inventory[ii].amount + "\n";
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
        AsteroidContainer = GameObject.Find("Asteroids").transform;
        myRect = GetComponent<RectTransform>();
        makeNavigationList();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateTimer < 0f) makeNavigationList();
        updateTimer -= Time.deltaTime;
    }
}
