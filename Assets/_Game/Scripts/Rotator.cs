using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float rotationSpeed = 90.0f;

    private void Update()
    {
        Quaternion q = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, axis);
        transform.rotation *= q;
    }
}
