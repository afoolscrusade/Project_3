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
    public int dialogueSelected;
    public int audioNumber;

    public AudioSource audioSource;
    public AudioClip dialogueAudio1;
    public AudioClip dialogueAudio2;
    public AudioClip dialogueAudio3;
    public Animator animator;

    ThirdPersonMovement attack;


    void Start()
    {
        levelOneObject.SetActive(false);
        levelTwoObject.SetActive(false);
        levelThreeObject.SetActive(false);
        attack = FindObjectOfType<ThirdPersonMovement>();
        //PlayerPrefs.SetInt("Dialgoue Level", 1);
        //dialogueSelected = PlayerPrefs.GetInt("Dialgoue Level");
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
            //animator.SetBool("isTalking", false);


        /*if (audioNumber == 2 && dialogueSelected == 0)
        {
            audioSource.PlayOneShot(dialogueAudio2);
        }*/


    }
    public void TriggerDialogue()
    {
        attack.canAttack = false;
        if (audioNumber == 0 && dialogueSelected == 0)
        {
            audioSource.PlayOneShot(dialogueAudio1);
            audioNumber += 1;
        }


        if (dialogueSelected == 0)
        {
            FindObjectOfType<Dialogue_Manager>().StartDialogue(dialogue);

            if (GetComponent<Collider>().CompareTag("Flomph"))
            {

                levelOneObject.SetActive(true);

            }

            if (GetComponent<Collider>().CompareTag("Marjal"))
            {

                levelTwoObject.SetActive(true);

            }

            if (GetComponent<Collider>().CompareTag("Zoe"))
            {
                levelThreeObject.SetActive(true);
            }
        }


    }
    public void NewTriggerDialogue()
    {
        if (audioNumber == 0 && dialogueSelected == 1)
        {
            audioSource.PlayOneShot(dialogueAudio3);
        }

        if (dialogueSelected == 1)
        {
            FindObjectOfType<Dialogue_Manager>().StartNewDialogue(dialogue);

            if (GetComponent<Collider>().CompareTag("Flomph"))
            {

                levelOneObject.SetActive(true);

            }

            if (GetComponent<Collider>().CompareTag("Marjal"))
            {

                levelTwoObject.SetActive(true);

            }

            if (GetComponent<Collider>().CompareTag("Zoe"))
            {
                levelThreeObject.SetActive(true);
            }
        }
    }

    public void StopAnimation()
    {
        animator.SetBool("isTalking", false);
    }


}