using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : MonoBehaviour
{
    public Transform turretSwivel;
    public Transform[] barrelEnds;

    public float maxDegreesPerSecond = 35f;

    private PlayerStation MyStation;

    private Quaternion aim;

    // Start is called before the first frame update
    void Start()
    {
        MyStation = GetComponent<PlayerStation>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( MyStation.thisPlayer != null)
        { // current player is seated in this turret right now
            turretSwivel.rotation = Quaternion.Lerp(MyStation.thisPlayer.leftController.transform.rotation, MyStation.thisPlayer.rightController.transform.rotation, 0.5f);
            turretSwivel.Rotate(Vector3.left, -90);
        }
    }
}
