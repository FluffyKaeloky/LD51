using NodeCanvas.Framework;
using ParadoxNotion.Services;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMoveTo : ActionTask
{
    public BBParameter<Vector3> target = Vector3.zero;
    public BBParameter<Transform> targetTransform = null;

    public BBParameter<float> completionDistance = 0.15f;

    public bool damping = true;
    public float dampingDistance = 1.0f;

    public float acceleration = 1.0f;

    private Pawn pawn = null;
    private Seeker seeker = null;
    private Queue<Vector3> vectorPath = new Queue<Vector3>();

    private Vector3? currentTarget = null;

    private float currentAcceleration = 0.0f;

    protected override string OnInit()
    {
        pawn = agent.GetComponent<Pawn>();
        seeker = agent.GetComponent<Seeker>();

        return base.OnInit();
    }

    protected override async void OnExecute()
    {
        if (targetTransform != null)
            target = targetTransform.value.position;

        ABPath path = ABPath.Construct(pawn.transform.position, target.value);
        await seeker.StartPath(path);
                
        if (path.error)
        {
            EndAction(false);
            return;
        }

        vectorPath = new Queue<Vector3>(path.vectorPath);
        currentAcceleration = 0.0f;

        MonoManager.current.onFixedUpdate += OnFixedUpdate;
    }

    protected override void OnStop()
    {
        MonoManager.current.onFixedUpdate -= OnFixedUpdate;
    }

    private void OnFixedUpdate()
    {
        if (currentTarget == null)
        {
            if (vectorPath.Count == 0)
            {
                EndAction();
                return;
            }

            currentTarget = vectorPath.Dequeue();
        }
        float distance = Vector3.Distance(pawn.transform.position, currentTarget.Value);
        float dampingValue = 1.0f;

        if (damping && vectorPath.Count == 0 && distance <= dampingDistance)
            dampingValue = Mathf.Clamp(distance / dampingDistance, 0.05f, 1.0f);

        Vector3 direction = Vector3.ProjectOnPlane(currentTarget.Value - pawn.transform.position, Vector3.up).normalized * currentAcceleration * dampingValue;

        pawn.Move(direction.x, direction.z);

        if (distance <= completionDistance.value)
            currentTarget = null;

        currentAcceleration = Mathf.Clamp01(currentAcceleration + Time.fixedDeltaTime * acceleration);
    }
}