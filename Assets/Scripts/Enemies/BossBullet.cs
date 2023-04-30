using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonMovement player = other.GetComponent<ThirdPersonMovement>();
        if (other.gameObject.tag == "Player")
        {
            player.UpdateHealth(-2);
            Destroy(gameObject);
        }
    }
}
