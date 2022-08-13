using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : MonoBehaviour
{
    public Transform turretSwivel;
    public Transform gunMount;

    private PlayerStation MyStation;

    private Vector3 turretDirection;

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
            //turretDirection = transform.position - ((MyStation.thisPlayer.leftController.transform.forward + MyStation.thisPlayer.rightController.transform.forward)*0.5f);
            turretDirection = MyStation.thisPlayer.leftController.transform.rotation.eulerAngles;
            turretSwivel.rotation = Quaternion.Euler(0f,turretDirection.y, 0f);
            gunMount.rotation = Quaternion.Euler(turretDirection.x, 0f, 0f);
        }
    }
}
