using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerVrHud : MonoBehaviour
{
    // HUds to turn off and on fast
    public GameObject HudPersonal;
    public Transform HudSwivel;

    // cockpit HUD
    public GameObject HudCockPit;
    public TMP_Text HudCockpitAimName;
    public TMP_Text HudCockpitAimContent;
    private float HudCockpitTimer = 0f;

    // Turret HUD
    public GameObject HudTurret;
    public TMP_Text HudTurretAimName;
    public TMP_Text HudTurretAimContent;
    public Transform midRing;
    private float HudTurretTimer = 0f;

    // drone HUD
    public GameObject HudDrone;
    public TMP_Text HudDroneAimName;
    public TMP_Text HudDroneAimContent;
    private float HudDroneTimer = 0f;

    // aim text
    private string hudAimNameText;
    private string HudAimContentText;

    private void DoRayCast(Vector3 start,Vector3 direction)
    {
        hudAimNameText = "";
        HudAimContentText = "";
        Ray ray = new Ray(start+direction, direction*500f);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData))
        { // The Ray hit something!
            var AsteroidScript = hitData.collider.GetComponent<Asteroid>();
            if (AsteroidScript)
            { // we hit and asteroid
                hudAimNameText = "Mineable\n"+AsteroidScript.AsteroidName;
                HudAimContentText = AsteroidScript.Inventory.InventoryToString();
                return;
            }
            var EnemyScript = hitData.collider.GetComponent<EnemyShip>();
            if (EnemyScript)
            { // We hit an enemy
                hudAimNameText = "Enemy\n" + EnemyScript.EnemyName;
                HudAimContentText = EnemyScript.structureHP.ToString() + " / " + EnemyScript.structureHPMax.ToString();
                return;
            }
            var PlayerModule = hitData.collider.GetComponent<PlayerModule>();
            if (PlayerModule)
            { // we hit part of our own ship
                hudAimNameText = PlayerModule.myShip.ShipName+ "\n" + PlayerModule.moduleName;
                HudAimContentText = PlayerModule.structureHP.ToString() + " / " + PlayerModule.structureMaxHP.ToString();
                return;
            }
        }
    }

    public void setHudDroneSwivel(Vector3 newPos,Vector3 newForward, Quaternion newRotation)
    {
        HudDroneTimer = 0f;
        HudDrone.SetActive(true);
        HudSwivel.rotation = newRotation;
        HudSwivel.localPosition = new Vector3(0f, 1.2f, 1f);
        DoRayCast(newPos,newForward);
        HudDroneAimName.text = hudAimNameText;
        HudDroneAimContent.text = HudAimContentText;
    }

    public void setHudCockpitSwivel(Vector3 newPos, Vector3 newForward, Quaternion newRotation)
    {
        HudCockpitTimer = 0f;
        HudCockPit.SetActive(true);
        HudSwivel.rotation = newRotation;
        HudSwivel.localPosition = new Vector3(0f, 1.2f, 1f);
        DoRayCast(newPos, newForward);
        HudCockpitAimName.text = hudAimNameText;
        HudCockpitAimContent.text = HudAimContentText;
    }

    public void setHudTurretSwivel(Vector3 newPos, Vector3 newForward, Quaternion newRotation)
    {
        HudTurretTimer = 0f;
        HudTurret.SetActive(true);
        HudSwivel.rotation = newRotation;
        midRing.localRotation = Quaternion.Euler(0,0,HudSwivel.localRotation.z*-100f);
        HudSwivel.position = Camera.main.transform.position+newForward;
        DoRayCast(newPos, newForward);
        HudTurretAimName.text = hudAimNameText;
        HudTurretAimContent.text = HudAimContentText;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HudDroneTimer += Time.deltaTime;
        HudCockpitTimer += Time.deltaTime;
        HudTurretTimer += Time.deltaTime;
        if (HudDroneTimer > 0.5f && HudDrone.activeSelf) HudDrone.SetActive(false);
        if (HudCockpitTimer > 0.5f && HudCockPit.activeSelf) HudCockPit.SetActive(false);
        if (HudTurretTimer > 0.5f && HudTurret.activeSelf) HudTurret.SetActive(false);
    }
}
