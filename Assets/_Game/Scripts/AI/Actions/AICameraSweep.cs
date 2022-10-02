using DG.Tweening;
using NodeCanvas.Framework;
using ParadoxNotion.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICameraSweep : ActionTask
{
    public BBParameter<Transform> startTarget = new BBParameter<Transform>();
    public BBParameter<Transform> endTarget = new BBParameter<Transform>();

    public BBParameter<float> timeForFullSweep = 2.0f;
    public BBParameter<float> pauseTimeAtExtremums = 1.0f;

    //private Vector3 currentAngle = Vector3.zero;

    private Pawn pawn = null;

    private Sequence sweepSequence = null;

    protected override string OnInit()
    {
        pawn = agent.GetComponent<Pawn>();
        //currentAngle = agent.transform.forward;

        sweepSequence = DOTween.Sequence(pawn.gameObject);

        sweepSequence
            .AppendInterval(pauseTimeAtExtremums.value / 2.0f)
            .Append(DOTween.To(x =>
            {
                Vector3 startAngle = (startTarget.value.position - pawn.transform.position).normalized;
                Vector3 endAngle = (endTarget.value.position - pawn.transform.position).normalized;

                Vector3 targetAngle = Vector3.Lerp(startAngle, endAngle, x);

                Debug.DrawLine(pawn.transform.position, pawn.transform.position + targetAngle, Color.blue);
                pawn.LookTo(new Vector2(targetAngle.x, targetAngle.z));
            }, 0.0f, 1.0f, timeForFullSweep.value)
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Fixed))
            .AppendInterval(pauseTimeAtExtremums.value / 2.0f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(UpdateType.Fixed);

        return base.OnInit();
    }

    protected override void OnExecute()
    {
        sweepSequence.Play();
    }

    protected override void OnStop()
    {
        sweepSequence.Pause();
    }
}
