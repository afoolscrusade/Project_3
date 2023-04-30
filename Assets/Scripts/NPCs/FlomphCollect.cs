using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlomphCollect : MonoBehaviour
{
    public Animator animator;
    public bool isBossDead;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBossDead == false)
        {
            animator.SetBool("isSad", true);
        }
        else if (isBossDead == true)
        {
            animator.SetBool("isSad", false);
        }
    }

     public void UpdateBoss()
    {
        isBossDead = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isBossDead == true)
            {
                PlayerPrefs.SetInt("Dialgoue Level", 1);
                SceneManager.LoadScene("MainHub");
            }
        }

    }
}
