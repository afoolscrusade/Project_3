using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyBullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonMovement player = other.GetComponent<ThirdPersonMovement>();
        if (other.gameObject.tag == "Player")
        {
            player.UpdateHealth(-1);
            Destroy(gameObject);
        }
    }
}
