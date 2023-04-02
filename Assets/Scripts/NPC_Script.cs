using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Script : MonoBehaviour
{
    public GameObject dialogueBox;



    void Start()
    {
        dialogueBox.SetActive(false);

        
    }

    void Update()
    
    {
        //added pickup button to this script to make it work
        if (Input.GetKeyDown("e"))
        {
            Debug.Log("Pickup");
            
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
                if (collider.TryGetComponent(out ItemPickup ItemPickup))
                {
                    ItemPickup.Pickup();
                    
                }
        }
    }
    
   
    
    public void Interact()
    {
        //Debug.Log("Interact");      
        dialogueBox.SetActive(true);


    }
}