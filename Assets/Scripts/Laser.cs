using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject laserPrefab;
    public GameObject firePoint;

    private GameObject spawnedLaser;

    // Start is called before the first frame update
    void Start()
    {
        spawnedLaser = Instantiate (laserPrefab, firePoint.transform) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {      

        
        if(Input.GetButtonDown("Fire2"))
        {
           EnableLaser();
        }
        if(Input.GetButton("Fire2"))
        {
            UpdateLaser();
        }
        if(Input.GetButtonUp("Fire2"))
        {
            DisableLaser();
        }

    }

    void EnableLaser()
    {
        spawnedLaser.SetActive (true);
    }

    void UpdateLaser()
    {
        if(firePoint != null)
        {
        spawnedLaser.transform.position = firePoint.transform.position;
        }
    }
    void DisableLaser()
    {
        spawnedLaser.SetActive (false);
    }
}
