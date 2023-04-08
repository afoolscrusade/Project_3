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

    public void QuitGame ()
    {
        Debug.Log("Quit");
        Application.Quit();

    }
}
