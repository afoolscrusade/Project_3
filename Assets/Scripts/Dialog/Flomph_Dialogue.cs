using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flomph_Dialogue : MonoBehaviour
{
    public Dialogue dialogue;

    public GameObject levelOneObject;

    //public int dialogueSelected;
    public int audioNumber;

    public AudioSource audioSource;
    public AudioClip dialogueAudio1;
    public AudioClip dialogueAudio2;
    public AudioClip dialogueAudio3;
    public AudioClip dialogueAudio4;
    public Animator animator;

    ThirdPersonMovement attack;


    public int FlomphLevel;



    void Start()
    {
        levelOneObject.SetActive(false);

        attack = FindObjectOfType<ThirdPersonMovement>();

        FlomphLevel = PlayerPrefs.GetInt("FlomphLevel");

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
        
        if (audioNumber == 0 && FlomphLevel == 0)
        {
            audioSource.PlayOneShot(dialogueAudio1);
            audioNumber += 1;

            FindObjectOfType<Dialogue_Manager>().StartDialogue(dialogue);
            levelOneObject.SetActive(true);
        }


    }
    public void NewTriggerDialogue()
    {
        if (audioNumber == 0 && FlomphLevel == 1)
        {
            audioSource.PlayOneShot(dialogueAudio3);


            FindObjectOfType<Dialogue_Manager>().StartNewDialogue(dialogue);
        }
    }

    public void StopAnimation()
    {
        animator.SetBool("isTalking", false);
    }


}