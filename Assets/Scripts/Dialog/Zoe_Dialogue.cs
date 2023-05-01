using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Zoe_Dialogue: MonoBehaviour
{
    public Dialogue dialogue;

    public GameObject levelThreeObject;
    //public int dialogueSelected;
    public int audioNumber;

    public AudioSource audioSource;
    public AudioClip dialogueAudio1;
    public AudioClip dialogueAudio2;
    public AudioClip dialogueAudio3;
    public AudioClip dialogueAudio4;
    public Animator animator;

    public GameObject button;
    public GameObject button1;

    ThirdPersonMovement attack;


    public int ZoeLevel;
    public int MarjalLevel;
    public int FlomphLevel;


    void Start()
    {
        button.SetActive(false);
        button1.SetActive(false);
        levelThreeObject.SetActive(false);
        attack = FindObjectOfType<ThirdPersonMovement>();

        ZoeLevel = PlayerPrefs.GetInt("ZoeLevel");

    }
    private void Update()
    {
        MarjalLevel = PlayerPrefs.GetInt("MarjalLevel");
        FlomphLevel = PlayerPrefs.GetInt("FlomphLevel");
        if (FlomphLevel == 1 && MarjalLevel == 1)
        {
            ZoeLevel = 1;
        }

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
                    ThirdTriggerDialogue();
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
        if (audioNumber == 0 && ZoeLevel == 0)
        {
            FindObjectOfType<Dialogue_Manager>().StartDialogue(dialogue);
        }

    }
    public void NewTriggerDialogue()
    {
        if (audioNumber == 0 && ZoeLevel == 1)
        {
            audioSource.PlayOneShot(dialogueAudio1);
            audioNumber += 1;

            FindObjectOfType<Dialogue_Manager>().StartNewDialogue(dialogue);
            button.SetActive(true);


            levelThreeObject.SetActive(true);

            PlayerPrefs.SetInt("MarjalLevel", 0);
            PlayerPrefs.SetInt("FlomphLevel", 0);

        }
    }
    public void ThirdTriggerDialogue()
    {
        button.SetActive(false);
        if (audioNumber == 0 && ZoeLevel == 2)
        {
            audioSource.PlayOneShot(dialogueAudio3);
            

            FindObjectOfType<Dialogue_Manager>().StartThirdDialogue(dialogue);
            button1.SetActive(false);
            PlayerPrefs.SetInt("ZoeLevel", 0);
            PlayerPrefs.SetInt("AquaricaLevel", 1);

        }
    }

    public void StopAnimation()
    {
        animator.SetBool("isTalking", false);
    }


}