using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ListNavigation : MonoBehaviour
{

    public PlayerShip myShip;

    public GameObject NavigationListItemPrefab;

    public float spacing = 55f;

    public Color trackOnColor;
    public Color trackOffColor;

    private RectTransform myRect;

    private float updateTimer = 1f;

    private Transform AsteroidContainer;
    private Asteroid asteroidScript;
    private float distanceCalc;
    private int siblingIndex;
    private bool alreadySorted = false;

    [Tooltip("Drag the audiosource for the buttonsound here")]
    public AudioSource ButtonSound;

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
            alreadySorted = false;
            foreach (Transform asteroid in AsteroidContainer)
            {
                asteroidScript = asteroid.GetComponent<Asteroid>();
                //object ref
                distanceCalc = Vector3.Distance(asteroidScript.ThePlayersShip.transform.position, asteroidScript.transform.position);
                siblingIndex = asteroid.transform.GetSiblingIndex();
                var listItem = Instantiate(NavigationListItemPrefab, transform);
                
                // listItem.transform.GetChild(0).GetComponent<RawImage>().texture = recipeFilter.RecipeList[i].recipeIcon;
                listItem.transform.GetChild(1).GetComponent<TMP_Text>().text = asteroidScript.AsteroidName;
                listItem.transform.GetChild(2).GetComponent<TMP_Text>().text = distanceCalc.ToString("F1") + " M";
                //string keepName = recipeFilter.RecipeList[i].recipeName;
                int astId = asteroidScript.getID();
                var button = listItem.transform.GetChild(4).GetComponent<Button>();
                button.onClick.AddListener(delegate { ToggleTracking(astId); });
                TMP_Text trackStatus = listItem.transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>();
                if (asteroidScript.trackThis)
                { // is tracking
                    trackStatus.text = "Track\nOn";
                    trackStatus.color = trackOnColor;
                } else { // is not tracking
                    trackStatus.text = "Track\nOff";
                    trackStatus.color = trackOffColor;
                }
                string usedUp = "";
                if (asteroidScript.Inventory.Inventory.Count > 0)
                {
                    for (int ii = 0; ii < asteroidScript.Inventory.Inventory.Count; ii++)
                    {
                        // denne her giver en index fejl
                        //Debug.Log("trying " + ii.ToString() + " of " + asteroidScript.Inventory.Inventory.Count());
                        usedUp += asteroidScript.Inventory.Inventory[ii].item.itemName + " : " + asteroidScript.Inventory.Inventory[ii].amount + "\n";
                    }
                }
                listItem.transform.GetChild(3).GetComponent<TMP_Text>().text = usedUp;
                if (AsteroidContainer.childCount > 5 && siblingIndex > 0 && !alreadySorted)
                {
                    float distance2 = Vector3.Distance(asteroidScript.ThePlayersShip.transform.position, AsteroidContainer.transform.GetChild(siblingIndex-1).transform.position);
                    if (distance2 > distanceCalc)
                    {
                        AsteroidContainer.transform.GetChild(siblingIndex - 1).SetSiblingIndex(siblingIndex);
                        asteroidScript.transform.SetSiblingIndex(siblingIndex - 1);
                        alreadySorted = true;
                    }
                }
            }
        }
        updateTimer = 1f;
    }

    public void ToggleTracking(int id)
    {
        if (AsteroidContainer.childCount > 0)
        {
            foreach (Transform asteroid in AsteroidContainer)
            {
                asteroidScript = asteroid.GetComponent<Asteroid>();
                int astId = asteroidScript.getID();
                if (astId == id)
                {
                    if (asteroidScript.trackThis)
                        asteroidScript.myPV.RPC("TrackAsteroid", RpcTarget.All, false);
                    else
                        asteroidScript.myPV.RPC("TrackAsteroid", RpcTarget.All, true);
                }
            }
        }
        if (ButtonSound) ButtonSound.Play();
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
