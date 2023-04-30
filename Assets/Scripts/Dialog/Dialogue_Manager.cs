using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Manager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public GameObject dialogueBox;

    
    private Queue<string> sentences;
    private Queue<string> newSentences;
    private Queue<string> thirdSentences;

    ThirdPersonMovement attack;
    Dialogue_Trigger npcAnimation;



  

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        newSentences = new Queue<string>();
        thirdSentences = new Queue<string>();
        attack = FindObjectOfType<ThirdPersonMovement>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
       //dialogueNumber.audioNumber += 1;
        nameText.text = dialogue.name;
        

        sentences.Clear();
 

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }


        DisplayNextSentence();
    }

    public void StartNewDialogue(Dialogue dialogue)
    {
        //dialogueNumber.audioNumber = 0;
        nameText.text = dialogue.name;

        newSentences.Clear();

        foreach (string newSentence in dialogue.newSentences)
        {
            newSentences.Enqueue(newSentence);
        }

        DisplayNextNewSentence();
    }

    public void StartThirdDialogue(Dialogue dialogue)
    {
        //dialogueNumber.audioNumber = 0;
        nameText.text = dialogue.name;

        thirdSentences.Clear();

        foreach (string thirdSentence in dialogue.thirdSentences)
        {
            thirdSentences.Enqueue(thirdSentence);
        }

        DisplayNextThirdSentence();
    }

    public void DisplayNextSentence()
    {
        //dialogueNumber.audioNumber += 1;
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void DisplayNextNewSentence()
    {
        if(newSentences.Count == 0)
        {
            FindObjectOfType<Dialogue_Trigger>().StopAnimation();
            EndDialogue();
            return;
        }
        //dialogueNumber.audioNumber += 1;

        string newSentence = newSentences.Dequeue();
        dialogueText.text = newSentence;
    }

    public void DisplayNextThirdSentence()
    {
        if(thirdSentences.Count == 0)
        {
            FindObjectOfType<Dialogue_Trigger>().StopAnimation();
            EndDialogue();
            return;
        }
        //dialogueNumber.audioNumber += 1;

        string thirdSentence = thirdSentences.Dequeue();
        dialogueText.text = thirdSentence;
    }    

    void EndDialogue()
    {
        Debug.Log("End");
        dialogueBox.SetActive(false);
        attack.canAttack = true;
        //npcAnimation.animator.SetBool("isTalking", false);
        //dialogueNumber.audioNumber = 0;
    }

}
