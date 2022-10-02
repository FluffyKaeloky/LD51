using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIScreenFader : MonoBehaviour
{
    private Image image = null;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void FadeIn(Action callback = null)
    {
        Color c = image.color;
        c.a = 0.0f;
        image.color = c;

        image.DOFade(1.0f, 1.0f)
            .OnComplete(() => callback?.Invoke());
    }

    public void FadeOut(Action callback = null)
    {
        Color c = image.color;
        c.a = 1.0f;
        image.color = c;

        image.DOFade(0.0f, 1.0f)
            .OnComplete(() => callback?.Invoke());
    }

    public void SnapTo(float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }
}
