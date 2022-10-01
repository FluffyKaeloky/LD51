using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotatingObject : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] Vector3 rotationVector;

    private void Start()
    {
        rotationVector = new Vector3(0, rotationSpeed, 0);
    }

    private void FixedUpdate()
    {

        transform.Rotate(rotationVector);
    }
}
