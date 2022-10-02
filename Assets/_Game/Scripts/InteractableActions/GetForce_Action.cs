using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class GetForce_Action : MonoBehaviour
{
    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    public void GetForce(Vector3 vecPlayerPos)
    {   
        //Vector3 vectorDirection = (transform.position - vecPlayerPos) * -1;
        rb.AddExplosionForce(10, new Vector3(5f,5f,5f), 10);
    }
}
