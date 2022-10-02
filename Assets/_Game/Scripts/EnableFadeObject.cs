using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnableFadeObject : MonoBehaviour
{

    [SerializeField] Color endColor;
    [SerializeField] Material myMaterial;

    private void Start()
    {
        myMaterial = GetComponent<Material>();
    }

    private async void OnEnable()
    {
        await new WaitForSeconds(15.0f);
        transform.DOScale(0.0f, 2.0f).OnComplete(() => Destroy(gameObject)).SetEase(Ease.OutExpo);
      //  myMaterial.DOColor(endColor, 30.0f).OnComplete(() => transform.DOScale(0,2.0f));
        

        Destroy(gameObject, 1.1f);
    }
}
