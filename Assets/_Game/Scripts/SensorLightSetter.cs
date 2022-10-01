using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using ExtendedColliders3D = ExtendedColliders3D.ExtendedColliders3D;

[RequireComponent(typeof(ExtendedColliders3D.ExtendedColliders3D))]
public class SensorLightSetter : MonoBehaviour
{
    public new Light light = null;

    private async void Start()
    {

    }
}
