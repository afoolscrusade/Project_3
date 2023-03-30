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

    }
    
   
    
    public void Interact()
    {
        //Debug.Log("Interact");      
        dialogueBox.SetActive(true);


    }
}