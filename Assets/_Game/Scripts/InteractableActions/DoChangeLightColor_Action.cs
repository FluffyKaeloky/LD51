using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Light))]
public class DoChangeLightColor_Action : MonoBehaviour
{
    [SerializeField] private Light mylight;
    [SerializeField] private Color endColor;

    private void Start()
    {
        mylight = GetComponent<Light>();
    }

    public void ChangeLightColor()
    {
        mylight.color = endColor;
    }
}
