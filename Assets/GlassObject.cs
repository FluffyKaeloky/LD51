using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(Collider))]
public class GlassObject : MonoBehaviour
{
    [SerializeField] private GameObject baseModel, BeakedModel;

    private Collider myCollider;
    public UnityEvent onGlassBreak = new UnityEvent();

    private void Start()
    {
        baseModel.SetActive(true);
        BeakedModel.SetActive(false);
        myCollider = GetComponent<Collider>();
    }

    public void BreakGlass()
    {
        baseModel.SetActive(false);
        BeakedModel.SetActive(true);
        myCollider.enabled = false;

        onGlassBreak?.Invoke();
    }
}
