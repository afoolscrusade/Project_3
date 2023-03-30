using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Trigger : MonoBehaviour
{
    public Dialogue dialogue;

    private void Update()
    {
        //NPC Interact
        if (Input.GetKeyDown("x"))
        {
            Debug.Log("NPC");
            
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
                if (collider.TryGetComponent(out NPC_Script NPC_Script))
                {
                    NPC_Script.Interact();
                    TriggerDialogue();
                }
        }


    }
    
    public void TriggerDialogue()
    {

        FindObjectOfType<Dialogue_Manager>().StartDialogue(dialogue);

    }

}
