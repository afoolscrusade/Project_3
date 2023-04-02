using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        DestroyProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
    void DestroyProjectile()
    {
        Destroy(gameObject, lifeTime);
    }
}
