using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void MainMenu ()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }    
    public void Hub()
    {
        SceneManager.LoadScene("MainHub");
    }

    public void QuitGame ()
    {
        Debug.Log("Quit");
        Application.Quit();

    }
}
