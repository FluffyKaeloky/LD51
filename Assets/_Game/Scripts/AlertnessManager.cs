using DG.Tweening;
using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.AI;

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

    public Vector3 LastKnownPlayerLocation { get; set; } = Vector3.zero;

    public float alertTime = 1.0f;
    public float forgetDelay = 1.0f;
    public float forgetRate = 1.0f;

    public float alertRangeGain = 5.0f;

    public bool reactToAlerts = true;

    public UnityEvent onChase = new UnityEvent();
    public UnityEvent onChaseEnd = new UnityEvent();

    public float Alertness { get => alertness; }
    private float alertness = 0.0f;
    private float lastSeenTime = 0.0f;

    private float baseFovRadius = 0.0f;
    private float highFovRadius = 0.0f;

    private void Start()
    {
        baseFovRadius = fieldOfView.viewRadius;
        highFovRadius = baseFovRadius + alertRangeGain;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameStates.Game)
            return;

        if (HasPlayerLOS)
        {
            /*if (AlertState == AlertStates.Alerted)
                alertness = 1.0f;*/

            alertness += Time.deltaTime;

            if (alertness >= alertTime && AlertState != AlertStates.Chasing)
            {
                AlertState = AlertStates.Chasing;
                AlertedNotifier.Instance.PushChaseAlert(this);
                onChase?.Invoke();
                DOTween.To(x => { fieldOfView.viewRadius = baseFovRadius + (alertRangeGain * x); }, 0.0f, 1.0f, 1.0f);
            }

            lastSeenTime = Time.time;
            LastKnownPlayerLocation = PlayerTransform.position;

            return;
        }
        else if (AlertState == AlertStates.Alerted)
        {
            if (Time.time - lastSeenTime >= forgetDelay)
                alertness = Mathf.Max(alertness - forgetRate * Time.deltaTime, 0.90f * alertTime);
        }

        if (AlertState == AlertStates.NoAlert)
        {
            if (Time.time - lastSeenTime >= forgetDelay)
                alertness = Mathf.Max(alertness - forgetRate * Time.deltaTime, 0.0f);
        }
    }

    public void ResetAlertState()
    {
        AlertStates oldState = AlertState;
        AlertState = AlertStates.NoAlert;

        if (oldState == AlertStates.Chasing)
        {
            DOTween.To(x => { fieldOfView.viewRadius = baseFovRadius - (alertRangeGain * (1.0f - x)); }, 0.0f, 1.0f, 1.0f);
            AlertedNotifier.Instance.DropChaseAlert(this);
            onChaseEnd?.Invoke();
        }
    }

    public void BecomeAlerted(Vector3? source)
    {
        if (!reactToAlerts)
            return;

        if (AlertState != AlertStates.NoAlert)
            return;

        AlertState = AlertStates.Alerted;
        if (source == null)
            LastKnownPlayerLocation = LevelManager.Instance.PlayerInput.transform.position;
        else
            LastKnownPlayerLocation = source.Value;
    }

    public void BecomeAlerted() =>
        BecomeAlerted(null);
}
