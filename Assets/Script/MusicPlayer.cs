using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip menuMusic;
    [SerializeField]
    private AudioClip levelMusic;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        // play Assets/SFX/The Graveyard (LOOP).wav  if the scene name is MainMenu
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            audioSource.Stop();
            audioSource.clip = menuMusic;

        }
        else
        {
            audioSource.Stop();
            audioSource.clip = levelMusic;
        }
        if(!audioSource.isPlaying)
            audioSource.Play();

    }
}
