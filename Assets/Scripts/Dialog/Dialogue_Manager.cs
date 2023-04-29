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


  

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        newSentences = new Queue<string>();
        
    }

    public void StartDialogue(Dialogue dialogue)
    {

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
        nameText.text = dialogue.name;

        newSentences.Clear();

        foreach (string newSentence in dialogue.newSentences)
        {
            newSentences.Enqueue(newSentence);
        }

        DisplayNextNewSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
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
            EndDialogue();
            return;
        }
        string newSentence = newSentences.Dequeue();
        dialogueText.text = newSentence;
    }

    void EndDialogue()
    {
        Debug.Log("End");
        dialogueBox.SetActive(false);
        

    }

}
