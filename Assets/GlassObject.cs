using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlassObject : MonoBehaviour
{
    [SerializeField] private GameObject baseModel, BeakedModel;

    public UnityEvent onGlassBreak = new UnityEvent();

    public void BreakGlass()
    {
        baseModel.SetActive(false);
        BeakedModel.SetActive(false);

        onGlassBreak?.Invoke();
    }
}
