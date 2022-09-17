using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ListCommunication : MonoBehaviour
{
    public PlayerShip myShip;

    public GameObject CommunicationListItemPrefab;

    public float spacing = 55f;

    private RectTransform myRect;

    private float updateTimer = 1f;

    private Transform EnemyContainer;
    private EnemyShip enemyShipScript;

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

    void makeCommunicationList()
    {
        clearTransformChildren(myRect.transform);
        myRect.sizeDelta = new Vector2(0, EnemyContainer.childCount * spacing);
        if ( EnemyContainer.childCount > 0)
        {
            foreach(Transform enemy in EnemyContainer)
            {
                enemyShipScript = enemy.GetComponent<EnemyShip>();
                var listItem = Instantiate(CommunicationListItemPrefab, transform);
                listItem.transform.GetChild(1).GetComponent<TMP_Text>().text = enemyShipScript.EnemyName;
                listItem.transform.GetChild(2).GetComponent<TMP_Text>().text = Vector3.Distance(enemyShipScript.myGM.myShip.transform.position, enemyShipScript.transform.position).ToString("F1") + " M";
            }
        }
        updateTimer = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyContainer = GameObject.Find("Enemies").transform;
        myRect = GetComponent<RectTransform>();
        makeCommunicationList();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateTimer < 0f) makeCommunicationList();
        updateTimer -= Time.deltaTime;
    }
}
