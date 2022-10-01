using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; } = null;

    public float defaultShakeAmplitude = 5.0f;

    private CinemachineBasicMultiChannelPerlin perlin = null;

    private Tweener tween = null;

    private void Awake()
    {
        Instance = this;

        perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float time)
    {
        Shake(time, defaultShakeAmplitude);
    }

    public void Shake(float time, float amplitude)
    {
        StopShake();

        tween = DOTween.To(x => perlin.m_AmplitudeGain = x, 0.0f, amplitude, time / 2.0f)
            .SetLoops(2, LoopType.Yoyo);
    }

    public void StopShake()
    {
        if (tween != null && tween.active)
            tween.Kill();
    }
}
