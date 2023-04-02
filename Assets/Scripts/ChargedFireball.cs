using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedFireball : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private GameObject fireball;
    [SerializeField]
    private Rigidbody rb;
    private void Start()
    {
        rb.velocity = Camera.main.transform.forward * speed;
        DestroyFireball();
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        Destroy(gameObject);
    }

    void DestroyFireball()
    {
        Destroy(gameObject, lifeTime); // destroys fireball after lifetime to save memory
    }
}