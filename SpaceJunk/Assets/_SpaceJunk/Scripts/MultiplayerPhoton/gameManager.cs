using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public class gameManager : MonoBehaviour
{

    List<UnityEngine.XR.InputDevice> leftHandDevices = new List<UnityEngine.XR.InputDevice>();
    List<UnityEngine.XR.InputDevice> rightHandDevices = new List<UnityEngine.XR.InputDevice>();


    public GameObject myXrRig;
    public Transform testSeat;
    public List<Transform> testSeats;
    public int seat = 0;

    public Transform AsteroidContainer;
    public List<GameObject> AsteroidPrefabs;
    public List<GameObject> SpawnedAsteroids;
    public int PreferedAsteroidCount = 100;


    private Vector3 spawnPosition;

    List<UnityEngine.XR.InputDevice> headDevices = new List<UnityEngine.XR.InputDevice>();
    public GameObject refUI;
    public GameObject refUISeat;

    private float nextButtonTimer = 0f;
    public void CreatePlayer()
    {
        Debug.Log("Creating player");
        var player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
        myXrRig.transform.position = testSeat.position;
        myXrRig.transform.SetParent(testSeat);
    }
    public async void CreateOtherPlayer()
    {
        myXrRig.transform.position = testSeat.position;
        myXrRig.transform.SetParent(testSeat);
        // while( true )
        // {
        /*await Task.Delay(1000);
            if ( GameObject.Find("PlayerShip(Clone)") != null )
            {
                myXrRig.transform.SetParent(GameObject.Find("PlayerShip(Clone)").transform);
                return;
            }*/
            await Task.Yield();

       // }       
    }

    void spawnAsteroid()
    {
        spawnPosition = new Vector3(Random.Range(-200f, 200f), Random.Range(-200f, 200f), Random.Range(-200f, 200f));
        if (spawnPosition.x > -100f && spawnPosition.x < 100f &&
            spawnPosition.y > -100f && spawnPosition.y < 100f &&
            spawnPosition.z > -100f && spawnPosition.z < 100f) return;
        string name = "rock" + Random.Range(1, 5);
        Debug.Log("Spawning " + name);
        var Asteroid = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", name), spawnPosition, Quaternion.identity);
        //Asteroid.SetActive(true);
        if ( Asteroid ) SpawnedAsteroids.Add(Asteroid);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.Head, headDevices);
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
        bool nextButton = false;
        if (leftHandDevices.Count > 0)
        {
            //leftHandDevices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTrigger);
            //leftHandDevices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out leftStick);
        }
        if (rightHandDevices.Count > 0)
        {
            rightHandDevices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out nextButton);
            //rightHandDevices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out rightStick);
        }

        if ( headDevices.Count > 0 )
        {// player has headset
            myXrRig.transform.position = testSeat.position;
            myXrRig.transform.SetParent(testSeat);
            refUI.SetActive(false);
        } else { // player have no headset
            myXrRig.transform.position = refUISeat.transform.position;
            myXrRig.transform.SetParent(refUISeat.transform);
            refUI.SetActive(true);
        }
        if ( nextButton && nextButtonTimer < 0f )
        {
            seat++;
            if (seat >= testSeats.Count) seat = 0;
            testSeat = testSeats[seat];
            myXrRig.transform.position = testSeat.position;
            myXrRig.transform.SetParent(testSeat);
            nextButtonTimer = 1f;
        }
        nextButtonTimer -= Time.deltaTime;
        if (SpawnedAsteroids.Count < PreferedAsteroidCount) spawnAsteroid();
    }

}
