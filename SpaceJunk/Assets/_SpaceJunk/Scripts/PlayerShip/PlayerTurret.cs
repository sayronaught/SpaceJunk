using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : MonoBehaviour
{
    public Transform turretSwivel;
    public Transform[] barrelEnds;
    private int currentBarrel = 0;
    private float shootDelay = 0f;
    public GameObject AmmoPrefab;

    public float maxDegreesPerSecond = 35f;

    private PlayerStation MyStation;
    private AudioSource myAS;

    private Quaternion aim;

    // Start is called before the first frame update
    void Start()
    {
        MyStation = GetComponent<PlayerStation>();
        myAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( MyStation.thisPlayer != null)
        { // current player is seated in this turret right now
            turretSwivel.rotation = Quaternion.Lerp(MyStation.thisPlayer.leftController.transform.rotation, MyStation.thisPlayer.rightController.transform.rotation, 0.5f);
            turretSwivel.Rotate(Vector3.left, -90);
            shootDelay -= Time.deltaTime;
            if ( shootDelay < 0f && (MyStation.thisPlayer.playerLeftTrigger || MyStation.thisPlayer.playerRightTrigger))
            {
                var shot = Instantiate(AmmoPrefab, barrelEnds[currentBarrel].position, barrelEnds[currentBarrel].rotation);
                shot.GetComponent<Rigidbody>().AddForce(barrelEnds[currentBarrel].forward * 10000f);
                Destroy(shot, 3000);
                myAS.Play();
                shootDelay = 0.5f;
            }
        }
    }
}
