using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerVrHud : MonoBehaviour
{
    // HUds to turn off and on fast
    public GameObject HudPersonal;
    public Transform HudSwivel;
    public GameObject HudCockPit;
    public GameObject HudTurret;

    // drone HUD
    public GameObject HudDrone;
    public TMP_Text HudDroneAimName;
    public TMP_Text HudDroneAimContent;

    private string hudAimNameText;
    private string HudAimContentText;

    private float HudDroneTimer = 0f;

    private void DoRayCast(Vector3 start,Vector3 direction)
    {
        hudAimNameText = "";
        HudAimContentText = "";
        Ray ray = new Ray(start+direction, direction*500f);
        RaycastHit hitData;
        Debug.DrawRay(ray.origin, ray.direction );
        if (Physics.Raycast(ray, out hitData))
        { // The Ray hit something!
            var AsteroidScript = hitData.collider.GetComponent<Asteroid>();
            if (AsteroidScript)
            { // we hit and asteroid
                hudAimNameText = "Asteroid\n"+AsteroidScript.AsteroidName;
                HudAimContentText = AsteroidScript.Inventory.InventoryToString();
                return;
            }
        }
    }

    public void setHudDroneSwivel(Vector3 newPos,Vector3 newForward, Quaternion newRotation)
    {
        HudDroneTimer = 0f;
        HudDrone.SetActive(true);
        HudSwivel.rotation = newRotation;
        DoRayCast(newPos,newForward);
        HudDroneAimName.text = hudAimNameText;
        HudDroneAimContent.text = HudAimContentText;
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
