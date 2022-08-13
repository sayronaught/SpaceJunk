using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public class gameManager : MonoBehaviour
{

    public PlayerVrControls vrControls;

    public GameObject myXrRig;
    public Transform testSeat;
    public List<PlayerStation> testSeats;
    public int seat = 0;

    public Transform AsteroidContainer;
    public List<GameObject> AsteroidPrefabs;
    public int PreferedAsteroidCount = 100;


    private Vector3 spawnPosition;

    public GameObject refUI;
    public GameObject refUISeat;

    private float nextButtonTimer = 0f;
    private PlayerVrControls myVrControls;

    public void CreatePlayer()
    {
        Debug.Log("Creating player");
        //var player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
        myXrRig.transform.position = testSeats[seat].transform.position;
        myXrRig.transform.SetParent(testSeats[seat].transform);
        testSeats[seat].addCurrentPlayer(myVrControls);
    }
    public async void CreateOtherPlayer()
    {
        myXrRig.transform.position = testSeats[seat].transform.position;
        myXrRig.transform.SetParent(testSeats[seat].transform);
        testSeats[seat].currentPlayers++;
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
        //Debug.Log("Spawning " + name);
        var Asteroid = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", name), spawnPosition, Quaternion.identity);
        Asteroid.transform.SetParent(AsteroidContainer);
    }

    void nextSeat()
    {
        seat++;
        if (seat >= testSeats.Count) seat = 0;
        if ( testSeats[seat].currentPlayers>=testSeats[seat].maxPlayers)
        {
            nextSeat();
            return;
        }
        testSeats[seat].addCurrentPlayer(myVrControls);
        testSeat = testSeats[seat].transform;
        myXrRig.transform.position = testSeat.position;
        myXrRig.transform.SetParent(testSeat);
        nextButtonTimer = 1f;
    }
    // Start is called before the first frame update
    void Start()
    {
        myVrControls = myXrRig.GetComponent<PlayerVrControls>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( vrControls.playerHasHeadSet )
        {// player has headset
            myXrRig.transform.position = testSeat.position;
            myXrRig.transform.SetParent(testSeat);
            refUI.SetActive(false);
        } else { // player have no headset
            myXrRig.transform.position = refUISeat.transform.position;
            myXrRig.transform.SetParent(refUISeat.transform);
            refUI.SetActive(true);
        }
        if (vrControls.playerRightPrimary && nextButtonTimer < 0f )
        {
            testSeats[seat].removeCurrentPlayer();
            nextSeat();
        }
        nextButtonTimer -= Time.deltaTime;

        // this bit only for the host
        if ( PhotonNetwork.IsMasterClient )
        {
            if (AsteroidContainer.childCount < PreferedAsteroidCount) spawnAsteroid();
        }
        
    }

}
