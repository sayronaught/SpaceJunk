using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmo : MonoBehaviour
{
    public float AmmoSpeed = 10000f;
    public float AmmoLifetime = 3f;

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
