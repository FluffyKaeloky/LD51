using DG.Tweening;
using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AlertnessManager : MonoBehaviour
{
    public enum AlertStates
    {
        NoAlert,
        Alerted,
        Chasing
    }

    public FieldOfView fieldOfView = null;

    public bool HasPlayerLOS { get => fieldOfView.visibleTargets.Any(x => x.root.tag == "Player"); }
    public Transform PlayerTransform { get => LevelManager.Instance.PlayerInput.transform; }

    public AlertStates AlertState { get; private set; } = AlertStates.NoAlert;

    public Vector3 LastKnownPlayerLocation { get; private set; } = Vector3.zero;

    public float alertTime = 1.0f;
    public float forgetDelay = 1.0f;
    public float forgetRate = 1.0f;

    public float alertRangeGain = 5.0f;

    public float Alertness { get => alertness; }
    private float alertness = 0.0f;
    private float lastSeenTime = 0.0f;

    private float baseFovRadius = 0.0f;

    private void Start()
    {
        baseFovRadius = fieldOfView.viewRadius;
    }

    private void Update()
    {
        if (HasPlayerLOS)
        {
            alertness += Time.deltaTime;

            if (alertness >= alertTime && AlertState != AlertStates.Chasing)
            {
                AlertState = AlertStates.Chasing;
                DOTween.To(x => { fieldOfView.viewRadius = baseFovRadius + alertRangeGain * x; }, 0.0f, 1.0f, 1.0f);
            }

            lastSeenTime = Time.time;
            LastKnownPlayerLocation = PlayerTransform.position;

            return;
        }

        if (AlertState == AlertStates.NoAlert)
        {
            if (Time.time - lastSeenTime >= forgetDelay)
                alertness = Mathf.Max(alertness - forgetRate * Time.deltaTime, 0.0f);
        }
    }

    public void ResetAlertState()
    {
        AlertState = AlertStates.NoAlert;
        float highRadius = fieldOfView.viewRadius;
        DOTween.To(x => { fieldOfView.viewRadius = highRadius - alertRangeGain * x; }, 0.0f, 1.0f, 1.0f);
    }
}