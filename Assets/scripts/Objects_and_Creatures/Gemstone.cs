using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gemstone : MonoBehaviour
{
    public AudioSource collectAudio;

    public void PlayCollectAudio()
    {
        collectAudio.Play();
    }
}
