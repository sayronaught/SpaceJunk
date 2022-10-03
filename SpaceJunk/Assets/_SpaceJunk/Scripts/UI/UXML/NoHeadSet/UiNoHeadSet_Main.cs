using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UiNoHeadSet_Main : MonoBehaviour
{
    private float updateTimer = 0f;

    private VisualElement myRoot;

    private Label labelSpeed;
    private Label labelMass;
    private Rigidbody shipRB;
    private float convert;

    // Start is called before the first frame update
    void Start()
    {
        myRoot = GetComponent<UIDocument>().rootVisualElement;
        shipRB = GameObject.Find("PlayerShip").GetComponent<Rigidbody>();
        labelSpeed = myRoot.Q<Label>("Speed");
        labelMass = myRoot.Q<Label>("Mass");
    }

    // Update is called once per frame
    void Update()
    {
        if (updateTimer < 0f)
        {
            convert = shipRB.mass * 0.001f;
            labelMass.text = convert.ToString("F1").Replace(".", ",") + " Tons";
            convert = shipRB.velocity.magnitude * 3.6f;
            labelSpeed.text = convert.ToString("F1").Replace(".", ",") + " km/h";
            updateTimer = 1f;
        }
        updateTimer -= Time.deltaTime;
    }
}
