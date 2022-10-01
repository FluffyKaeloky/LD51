using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractableZone : MonoBehaviour
{

    [SerializeField] private GameObject showSpritePivot;
    [SerializeField] private Outline outlineObject;

    [SerializeField] private bool isPlayerInside = false;
    [SerializeField] private bool canInteracte = true;

    public UnityEvent onIntecact = new UnityEvent();
    [SerializeField] private PlayerInput _PI = null;



    private void Start()
    {
        outlineObject.GetComponentInChildren<Outline>();

        isPlayerInside = false;
        ShowFeedBack(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (_PI == null)
        {
            _PI = other.GetComponentInParent<PlayerInput>();
        }

        if (canInteracte)
        {
            isPlayerInside = true;
            _PI.currentInteractZone = this;

            ShowFeedBack(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInside = false;
        _PI.currentInteractZone = null;

        ShowFeedBack(false);
    }

    public void Interact()
    {
        if (canInteracte)
        {
            canInteracte = false;
            ShowFeedBack(false);

            onIntecact?.Invoke();
        }
    }
    
    public void ShowFeedBack(bool state)
    {
        if (showSpritePivot != null) { showSpritePivot.SetActive(state); }
        if (outlineObject != null) { outlineObject.enabled = state; }
    }

}
