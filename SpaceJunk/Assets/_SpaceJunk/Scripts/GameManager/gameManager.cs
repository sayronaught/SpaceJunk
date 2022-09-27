using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public List<string> AsteroidPrefabName;
    public int PreferedAsteroidCount = 100;

    [Tooltip("The Transform all enemies will be added to")]
    public Transform EnemyContainer;
    public List<string> EnemyPrefabName;

    public GameObject PersonalHUDPrefab;

    public ListOfSoundEffects SoundBank;

    private Vector3 spawnPosition;

    public GameObject refUI;
    public GameObject refUISeat;

    private float nextButtonTimer = 0f;
    private PlayerVrControls myVrControls;

    // defines
    private float asteroidMax = 500f;
    private float asteroidMin = 200f;
    private float enemyMax = 1000f;
    private float enemyMin = 500f;

    private PhotonView myPV;

    [PunRPC]
    public void allPlayersReboot()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

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
        spawnPosition = new Vector3(Random.Range(-asteroidMax, asteroidMax), Random.Range(-asteroidMax, asteroidMax), Random.Range(-asteroidMax, asteroidMax));
        if (spawnPosition.x > -asteroidMin && spawnPosition.x < asteroidMin &&
            spawnPosition.y > -asteroidMin && spawnPosition.y < asteroidMin &&
            spawnPosition.z > -asteroidMin && spawnPosition.z < asteroidMin) return;
        var Asteroid = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", AsteroidPrefabName[Random.Range(0, AsteroidPrefabName.Count)]), myShip.transform.position + spawnPosition, Quaternion.identity);
        var asteroidScript = Asteroid.GetComponent<Asteroid>();
        asteroidScript.ThePlayersShip = myShip;
        asteroidScript.myGM = this;
        Asteroid.transform.SetParent(AsteroidContainer);
    }

    void spawnEnemy()
    {
        spawnPosition = new Vector3(Random.Range(-enemyMax, enemyMax), Random.Range(-enemyMax, enemyMax), Random.Range(-enemyMax, enemyMax));
        if (spawnPosition.x > -enemyMin && spawnPosition.x < enemyMin &&
            spawnPosition.y > -enemyMin && spawnPosition.y < enemyMin &&
            spawnPosition.z > -enemyMin && spawnPosition.z < enemyMin) return;
        var Enemy = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", EnemyPrefabName[Random.Range(0,EnemyPrefabName.Count)] ), myShip.transform.position + spawnPosition, Quaternion.identity);
        Enemy.transform.SetParent(EnemyContainer);
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
        myPV = GetComponent<PhotonView>();
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
            if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKey(KeyCode.Return)) myPV.RPC("allPlayersReboot", RpcTarget.All);
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
            //if (EnemyContainer.childCount < 5) spawnEnemy();
            if (EnemyContainer.childCount <  Time.timeSinceLevelLoad * 0.001f) spawnEnemy();
        }

    }

}
