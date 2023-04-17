using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dialogue_Trigger : MonoBehaviour
{
    public Dialogue dialogue;

    public GameObject levelOneObject;
    public GameObject levelTwoObject;
    public GameObject levelThreeObject;


    void Start()
    {
        levelOneObject.SetActive(false);
        levelTwoObject.SetActive(false);
        levelThreeObject.SetActive(false);
    }
    private void Update()
    {
        //NPC Interact
        if (Input.GetKeyDown("x") || Input.GetButtonDown("Interact"))
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

        if(GetComponent<Collider>().CompareTag("LevelOne"))
        {
            
            levelOneObject.SetActive(true);
            
        }

        if(GetComponent<Collider>().CompareTag("LevelTwo"))
        {
            
            levelTwoObject.SetActive(true);
            
        }
        
        if(GetComponent<Collider>().CompareTag("LevelThree"))
        {
            
            levelThreeObject.SetActive(true);
            
        }

    }

    

}
