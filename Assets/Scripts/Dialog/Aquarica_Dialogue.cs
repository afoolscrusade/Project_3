using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Aquarica_Dialogue : MonoBehaviour
{
    public Dialogue dialogue;

    public GameObject OutObject;


 
    public int audioNumber;

    public AudioSource audioSource;
    public AudioClip dialogueAudio1;
    public AudioClip dialogueAudio2;
    public AudioClip dialogueAudio3;
    public Animator animator;

    ThirdPersonMovement attack;

    public int AquaricaLevel;
 



    void Start()
    {
        attack = FindObjectOfType<ThirdPersonMovement>();
        AquaricaLevel = PlayerPrefs.GetInt("AquaricaLevel");
        OutObject.SetActive(false);
    }
    private void Update()
    {

        //NPC Interact
        if (Input.GetKeyDown("x") || Input.GetButtonDown("Interact"))
        {
            Debug.Log("NPC");
            //PlayerPrefs.GetInt("GoToMainHub");
            

            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
                if (collider.TryGetComponent(out NPC_Script NPC_Script))
                {
                    NPC_Script.Interact();

                    animator.SetBool("isTalking", true);
                    audioNumber = 0;
                    TriggerDialogue();
                    NewTriggerDialogue();
                    
                }
        }
    }

    public void NextDialgoueAudio()
    {

            if (audioNumber == 1)
            {
                audioSource.PlayOneShot(dialogueAudio2);
                audioNumber = 0;
            }



    }
    public void TriggerDialogue()
    {
        attack.canAttack = false;
        if (audioNumber == 0 && AquaricaLevel == 0)
        {
            audioSource.PlayOneShot(dialogueAudio1);
            

            FindObjectOfType<Dialogue_Manager>().StartDialogue(dialogue);
        }




    }
    public void NewTriggerDialogue()
    {
        if (audioNumber == 0 && AquaricaLevel == 1)
        {
            audioSource.PlayOneShot(dialogueAudio2);

            FindObjectOfType<Dialogue_Manager>().StartNewDialogue(dialogue);
            Invoke("SendToOut", 5);

        }
    }

    public void StopAnimation()
    {
        animator.SetBool("isTalking", false);
    }

    void SendToOut()
    {
        OutObject.SetActive(true);
    }

}