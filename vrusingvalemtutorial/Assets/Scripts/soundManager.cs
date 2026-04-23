using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class soundManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> soundClips; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Sound1() {
        audioSource.PlayOneShot(soundClips[0]);
    }
    public void Sound2()
    {
        audioSource.PlayOneShot(soundClips[1]);
    }
    public void Sound3()
    {
        audioSource.PlayOneShot(soundClips[2]);
    }
    public void Sound4()
    {
        audioSource.PlayOneShot(soundClips[3]);

    }
    public void Sound5()
    {
        audioSource.PlayOneShot(soundClips[4]);
        Invoke("Sound6", 12f);

    }

    public void Sound6()
    {
        audioSource.PlayOneShot(soundClips[5]);

    }
}
