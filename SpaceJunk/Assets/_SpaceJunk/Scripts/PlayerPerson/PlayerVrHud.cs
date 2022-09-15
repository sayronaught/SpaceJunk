using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVrHud : MonoBehaviour
{
    // HUds to turn off and on fast
    public GameObject HudPersonal;
    public Transform HudSwivel;
    public GameObject HudCockPit;
    public GameObject HudTurret;
    public GameObject HudDrone;

    private float HudDroneTimer = 0f;

    public void setHudDroneSwivel(Quaternion newRotation)
    {
        HudDroneTimer = 0f;
        HudDrone.SetActive(true);
        HudSwivel.rotation = newRotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HudDroneTimer += Time.deltaTime;
        if (HudDroneTimer > 0.5f && HudDrone.activeSelf) HudDrone.SetActive(false);
    }
}
