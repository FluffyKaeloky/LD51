using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSetter : MonoBehaviour
{
    public float menuState = 0.0f;
    public float sneakState = 0.0f;
    public float chaseState = 0.0f;

    private void Start()
    {
        BGMManager.Instance.SetMixerState(sneakState, chaseState, menuState, 1.0f);
    }
}
