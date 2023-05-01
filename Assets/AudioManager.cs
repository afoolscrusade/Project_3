using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    public float volume;
    // Start is called before the first frame update
    void Start()
    {
        volume = PlayerPrefs.GetFloat("Master Volume");
        mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
