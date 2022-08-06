using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public class gameManager : MonoBehaviour
{

    public GameObject myXrRig;
    public Transform testSeat;
    public List<Transform> testSeats;
    public int seat = 0;

    public Transform AsteroidContainer;
    public List<GameObject> AsteroidPrefabs;
    public List<GameObject> SpawnedAsteroids;
    public int PreferedAsteroidCount = 100;


    private Vector3 spawnPosition;

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
        string name = "rock" + Random.Range(0, 5);
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
        if (SpawnedAsteroids.Count < PreferedAsteroidCount) spawnAsteroid();
    }

}
