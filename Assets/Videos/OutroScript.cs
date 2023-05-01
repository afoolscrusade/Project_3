using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class OutroScript : MonoBehaviour
{
    VideoPlayer video;
    // Start is called before the first frame update
    void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.Play();
        video.loopPointReached += OnMovieEnded;
    }


    void OnMovieEnded(VideoPlayer vp)
    {
        SceneManager.LoadScene("WinScreen");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}