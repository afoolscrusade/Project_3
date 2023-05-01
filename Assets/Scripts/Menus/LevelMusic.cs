using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip levelMusic;
    public AudioClip levelAmbience;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.PlayOneShot(levelMusic);
        audioSource.PlayOneShot(levelAmbience);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
