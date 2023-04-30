using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dummies : MonoBehaviour
{
    public static int GoToHub;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GoToHub >= 2)
        {
            PlayerPrefs.SetInt("GoToMainHub", 1);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            
            //SceneManager.LoadScene("MainHub");
            GoToHub += 1;
            

        }
    }
}
