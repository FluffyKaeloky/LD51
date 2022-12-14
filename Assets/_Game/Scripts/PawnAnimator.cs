using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pawn))]
public class PawnAnimator : MonoBehaviour
{
    public string walkSpeedParameterName = "Walk";
    public Animator animator = null;

    public bool canBeKilled = false;

    private Pawn pawn = null;
    private AlertedNotifier chaseNotifier = null;

    private bool wasBeingChased = false;
    private bool currentChaseState = false;

    private void Awake()
    {
        pawn = GetComponent<Pawn>();
        chaseNotifier = GetComponent<AlertedNotifier>();
    }

    private void Start()
    {
        LevelManager.Instance.onLoss.AddListener(() => 
        {
            if (canBeKilled)
                animator.SetTrigger("Death"); 
        });
    }

    private void Update()
    {
        animator.SetFloat(walkSpeedParameterName, Mathf.Clamp01(pawn.Velocity.magnitude / pawn.moveSpeed));

        bool chaseState = chaseNotifier == null ? currentChaseState : chaseNotifier.IsBeingChased;

        if (chaseState && !wasBeingChased)
        {
            this.DOKill();
            DOTween.To(x => { animator.SetLayerWeight(1, x); }, animator.GetLayerWeight(1), 1.0f, 0.5f)
                .SetTarget(this);

            wasBeingChased = true;
        }
        else if (!chaseState && wasBeingChased)
        {
            this.DOKill();
            DOTween.To(x => { animator.SetLayerWeight(1, x); }, animator.GetLayerWeight(1), 0.0f, 0.5f)
                .SetTarget(this);

            wasBeingChased = false;
        }
    }

    public void SetAlertState(bool alerted)
    {
        currentChaseState = alerted;
    }
}
