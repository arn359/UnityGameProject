using UnityEngine;
using System.Collections;


public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioSource startmusic;

    public AudioSource redmusic;
    public AudioSource blackmusic;
    public AudioSource normalmusic;

    void Awake()
    {
        instance = this;
        if (startmusic != null) startmusic.Play();
    }

    public void PlayRed()
    {
        if (startmusic != null) startmusic.Stop();
        if (blackmusic != null) blackmusic.Stop();
        if (normalmusic != null) normalmusic.Stop();
        if (redmusic != null) redmusic.Play();
    }

    public void PlayBlack()
    {
        if (startmusic != null) startmusic.Stop();
        if (redmusic != null) redmusic.Stop();
        if (normalmusic != null) normalmusic.Stop();
        if (blackmusic != null) blackmusic.Play();
    }

    public void PlayNormal()
    
    {
       
        if (startmusic != null) startmusic.Play();
        if (redmusic != null) redmusic.Stop();
        if (blackmusic != null) blackmusic.Stop();
        if (normalmusic != null) normalmusic.Play();
    }
}