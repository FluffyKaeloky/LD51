using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(Collider))]
public class GlassObject : MonoBehaviour
{
    [SerializeField] private GameObject baseModel, beakedModel;

    private Collider myCollider;
    public UnityEvent onGlassBreak = new UnityEvent();
    public Rigidbody[] glassPiecesRB;

    private void Start()
    {
        baseModel.SetActive(true);
        beakedModel.SetActive(false);
        myCollider = GetComponent<Collider>();
        glassPiecesRB = beakedModel.GetComponentsInChildren<Rigidbody>();

    }

    public async void BreakGlass(float proutPower, Vector3 playerPos)
    {
        baseModel.SetActive(false);
        beakedModel.SetActive(true);
        myCollider.enabled = false;

        await new WaitForEndOfFrame();
        
        foreach (Rigidbody glassPiece in glassPiecesRB)
        {
            glassPiece.AddExplosionForce(proutPower *200, playerPos, proutPower * 5);
        }

        onGlassBreak?.Invoke();
    }
}
