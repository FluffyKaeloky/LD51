using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MouseUIDetector : MonoBehaviour
     , IPointerClickHandler // 2
     , IPointerEnterHandler
     , IPointerExitHandler
{
    public UnityEvent OnClicked;
    public UnityEvent OnPointerEnterAction;
    public UnityEvent OnPointerExitAction;


    public void OnPointerClick(PointerEventData eventData) // 3
    {
        print("Pointer clicked");
        OnClicked?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Pointer Enter");
        OnPointerEnterAction?.Invoke();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("Pointer Exit");
        OnPointerExitAction?.Invoke();

    }
}

