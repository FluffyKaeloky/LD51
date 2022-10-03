using AlmenaraGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public AudioObject audioObject = null;

    public void PlaySFX()
    {
        if (audioObject != null)
            MultiAudioManager.PlayAudioObject(audioObject, transform.position);
    }
}
