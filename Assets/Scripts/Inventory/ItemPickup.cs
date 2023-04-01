using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Debug.Log("Pickup");
            
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
                if (collider.TryGetComponent(out ItemPickup Itempickup))
                {
                    Pickup();
                }
        }
    }
    public void Pickup()
    {
        if(GetComponent<Collider>().CompareTag("Health"))
        {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
        }
    }
}
