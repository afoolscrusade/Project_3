using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Script : MonoBehaviour

{
public float displayTime = 4.0f;
public GameObject dialogBox;
float timerDisplay;

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
    }
    
    void Update()
    {
        if (timerDisplay >= 0)
        {
            //timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);

            }
        }
    }
    
    public void Interact()
    {
        Debug.Log("Interact");
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

    }
}