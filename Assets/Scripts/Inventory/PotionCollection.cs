using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCollection : MonoBehaviour
{
    public AudioClip collectedClip;

    
    void OnTriggerEnter(Collider other)
    {
        PlayerMovementTutorial controller = other.GetComponent<PlayerMovementTutorial>();

        //PlayerMovementTutorial.HealthP += 1;
        //controller.SetCurrentHP();
        Destroy(gameObject);            
        //controller.PlaySound(collectedClip);
    }
}
