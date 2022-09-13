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
    public PlayerShip myShip;
    public Transform testSeat;
    public List<PlayerStation> testSeats;
    public int seat = 0;

    public PlayerDroneController activeDrone;

    public Transform AsteroidContainer;
    public List<GameObject> AsteroidPrefabs;
    public int PreferedAsteroidCount = 100;

    public List<GameObject> SpawnedEnemies;
    public List<string> EnemyPrefabName;

    public ListOfSoundEffects SoundBank;

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
        await Task.Yield();
    }

    void spawnAsteroid()
    {
        spawnPosition = new Vector3(Random.Range(-500f, 500f), Random.Range(-500f, 500f), Random.Range(-500f, 500f));
        if (spawnPosition.x > -200f && spawnPosition.x < 200f &&
            spawnPosition.y > -200f && spawnPosition.y < 200f &&
            spawnPosition.z > -200f && spawnPosition.z < 200f) return;
        string name = "rock" + Random.Range(1, 5);
        var Asteroid = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", name), myShip.transform.position + spawnPosition, Quaternion.identity);
        Asteroid.GetComponent<Asteroid>().ThePlayersShip = myShip;
        Asteroid.transform.SetParent(AsteroidContainer);
    }

    void spawnEnemy()
    {
        spawnPosition = new Vector3(Random.Range(-600f, 600f), Random.Range(-600f, 600f), Random.Range(-600f, 600f));
        if (spawnPosition.x > -500f && spawnPosition.x < 500f &&
            spawnPosition.y > -500f && spawnPosition.y < 500f &&
            spawnPosition.z > -500f && spawnPosition.z < 500f) return;
        var Enemy = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", EnemyPrefabName[0] ), myShip.transform.position + spawnPosition, Quaternion.identity);
        Enemy.GetComponent<EnemyShip>().myGM = this;
        SpawnedEnemies.Add(Enemy);
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
        if ( testSeats[seat].DroneStation )
        { // Drone station
            var Drone = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", testSeats[seat].DronePrefabName), testSeat.position, testSeat.rotation);
            activeDrone = Drone.GetComponent<PlayerDroneController>();
            activeDrone.thisPlayer = myVrControls;
            activeDrone.thisStation = testSeats[seat];
            activeDrone.thisShip = myShip;
        } else { // regular seat
            activeDrone = null;
            myXrRig.transform.position = testSeat.position;
            myXrRig.transform.SetParent(testSeat);
        }
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
            refUI.SetActive(false);
            if ( activeDrone )
            { // move player into drone
                myXrRig.transform.position = activeDrone.transform.GetChild(0).position;
                myXrRig.transform.rotation = activeDrone.transform.GetChild(0).rotation;
                myXrRig.transform.SetParent(activeDrone.transform.GetChild(0));
            } else {
                myXrRig.transform.position = testSeat.position;
                myXrRig.transform.rotation = testSeat.rotation;
                myXrRig.transform.SetParent(testSeat);
            }
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
            //if (SpawnedEnemies.Count < 5) spawnEnemy();
            //if (SpawnedEnemies.Count < Time.timeSinceLevelLoad * 0.001) spawnEnemy();
        }
        
    }

}
