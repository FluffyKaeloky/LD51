using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractableZone : MonoBehaviour
{

    private Outline outlineObject;
    [SerializeField] private GameObject showInteractSpritePivot;
    [SerializeField] private GameObject showRequireSpritePivot;

    private bool isPlayerInside = false;
    
    [SerializeField] private bool isInstantInteracting;

    private bool canInteracte = true;
    [SerializeField] private bool isConditonRequired = false;

    private PlayerInput _PI = null;
    public UnityEvent onIntecact = new UnityEvent();
    public UnityEvent onGetRequireComponent = new UnityEvent();



    private void Start()
    {
        canInteracte = true;
        outlineObject = GetComponentInChildren<Outline>();
        isPlayerInside = false;

        //setup feedback
        if (showInteractSpritePivot != null) { showInteractSpritePivot.SetActive(false); }
        if (showRequireSpritePivot != null) { showRequireSpritePivot.SetActive(false); }
        if (isInstantInteracting) { ShowHoverInteractFeedBack(true); }
        else { ShowHoverInteractFeedBack(false); }

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
            ShowHoverInteractFeedBack(true);
        }

        if (isInstantInteracting)
        {
            isConditonRequired = false;
            Interact();
            return;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInside = false;
        ShowHoverInteractFeedBack(false);
    }

    public void Interact()
    {
        print("Interacted");

        if (canInteracte && !isConditonRequired && isPlayerInside)
        {
            canInteracte = false;
            ShowHoverInteractFeedBack(false);

            onIntecact?.Invoke();
        }

    }
    
    public void ShowHoverInteractFeedBack(bool state)
    {
        //Active Desactive Outline
        if (outlineObject != null) { outlineObject.enabled = state; }

        //Active Desactive Key Feedback
        if (isConditonRequired && showRequireSpritePivot != null)
        {
            showRequireSpritePivot.SetActive(state);
        }
        else if(showInteractSpritePivot != null)
        {
            showInteractSpritePivot.SetActive(state);
        }
    }


    public void ValidRequireCondition()
    {
        isConditonRequired = false;
        onGetRequireComponent?.Invoke();
    }

}
