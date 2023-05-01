using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_Dialogue : MonoBehaviour
{
    public Dialogue dialogue;

    public GameObject MainHubObject;
    public int GoToMainHub;

    public int dialogueSelected;
    public int audioNumber;

    public AudioSource audioSource;
    public AudioClip dialogueAudio1;
    public AudioClip dialogueAudio2;
    public AudioClip dialogueAudio3;
    public Animator animator;

    ThirdPersonMovement attack;
    Dummies dummy;


    void Start()
    {
        MainHubObject.SetActive(false);
        dummy = FindObjectOfType<Dummies>();

        attack = FindObjectOfType<ThirdPersonMovement>();
        //PlayerPrefs.SetInt("Dialgoue Level", 1);
        //dialogueSelected = PlayerPrefs.GetInt("Dialgoue Level");

        PlayerPrefs.SetInt("GoToMainHub", 0);
        PlayerPrefs.SetInt("FlomphLevel", 0);
        PlayerPrefs.SetInt("MarjalLevel", 0);
        PlayerPrefs.SetInt("ZoeLevel", 0);

    }
    private void Update()
    {
        
        GoToMainHub = PlayerPrefs.GetInt("GoToMainHub");
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
                    TutorialTriggerDialogue();
                    TutorialNewTriggerDialogue();
                    
                }
        }
    }

    public void TutorialNextDialgoueAudio()
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
    public void TutorialTriggerDialogue()
    {
        attack.canAttack = false;
        if (audioNumber == 0 && GoToMainHub == 0)
        {
            audioSource.PlayOneShot(dialogueAudio1);
            audioNumber += 1;
        }


        if (GoToMainHub == 0)
        {
            FindObjectOfType<Dialogue_Manager>().StartDialogue(dialogue);

            if (GetComponent<Collider>().CompareTag("Aquarica"))
            {
                
            }

        }


    }
    public void TutorialNewTriggerDialogue()
    {
        if (audioNumber == 0 && GoToMainHub == 1)
        {
            audioSource.PlayOneShot(dialogueAudio3);
        }

        if (GoToMainHub == 1)
        {
            FindObjectOfType<Dialogue_Manager>().StartNewDialogue(dialogue);

            if (GetComponent<Collider>().CompareTag("Aquarica"))
            {
                Invoke("SendToGame", 2);
            }
        }
    }

    public void TutorialStopAnimation()
    {
        animator.SetBool("isTalking", false);
    }

    void SendToGame()
    {
        MainHubObject.SetActive(true);
    }

}