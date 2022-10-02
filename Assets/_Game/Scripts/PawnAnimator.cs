using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pawn))]
public class PawnAnimator : MonoBehaviour
{
    public string walkSpeedParameterName = "Walk";
    public Animator animator = null;

    private Pawn pawn = null;
    private AlertedNotifier chaseNotifier = null;

    private bool wasBeingChased = false;

    private void Awake()
    {
        pawn = GetComponent<Pawn>();
        chaseNotifier = GetComponent<AlertedNotifier>();
    }

    private void Update()
    {
        animator.SetFloat(walkSpeedParameterName, pawn.RawInput.magnitude);

        if (chaseNotifier != null)
        {
            if (chaseNotifier.IsBeingChased && !wasBeingChased)
            {
                this.DOKill();
                DOTween.To(x => { animator.SetLayerWeight(1, x); }, animator.GetLayerWeight(1), 1.0f, 0.5f)
                    .SetTarget(this);

                wasBeingChased = true;
            }
            else if (!chaseNotifier.IsBeingChased && wasBeingChased)
            {
                this.DOKill();
                DOTween.To(x => { animator.SetLayerWeight(1, x); }, animator.GetLayerWeight(1), 0.0f, 0.5f)
                    .SetTarget(this);

                wasBeingChased = false;
            }
        }
    }
}
