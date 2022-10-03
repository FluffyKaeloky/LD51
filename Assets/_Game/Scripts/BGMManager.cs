using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; } = null;

    public AudioMixer mixer = null;

    public float initialSneak = 0.0f;
    public float intialChase = 0.0f;
    public float intialMenu = 0.0f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetMixerState(initialSneak, intialChase, intialMenu, 0.0f);
    }

    public void SetMixerState(float sneak, float chase, float menu, float duration)
    {
        mixer.DOSetFloat("SneakAttenuation", Mathf.Lerp(-60.0f, 0.0f, sneak), duration);
        mixer.DOSetFloat("ChaseAttenuation", Mathf.Lerp(-60.0f, 0.0f, chase), duration);
        mixer.DOSetFloat("MenuAttenuation", Mathf.Lerp(-60.0f, 0.0f, menu), duration);
    }
}
