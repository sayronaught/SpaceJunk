using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerAmmo : MonoBehaviour
{
    public float AmmoSpeed = 10000f;
    public float AmmoLifetime = 3f;

    private void OnCollisionEnter(Collision collision)
    {
        if ( PhotonNetwork.IsMasterClient )
        {
            var explo = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Explosion"), collision.GetContact(0).point, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * AmmoSpeed);
        Destroy(gameObject, AmmoLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
