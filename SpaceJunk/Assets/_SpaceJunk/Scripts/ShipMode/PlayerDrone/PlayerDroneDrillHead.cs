using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneDrillHead : MonoBehaviour
{
    public List<Asteroid> AsteroidsInRange;

    private void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Asteroid")
        {
            AsteroidsInRange.Add(other.GetComponent<Asteroid>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var check = other.GetComponent<Asteroid>();
        if (AsteroidsInRange.Contains(check))
        {
            AsteroidsInRange.Remove(check);
        }
    }

}
