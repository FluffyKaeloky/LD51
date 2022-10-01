using NodeCanvas.Framework;
using ParadoxNotion.Services;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PawnSeek : ActionTask
{
    public BBParameter<Transform> targetTransform = null;
    public float recomputeDistance = 1.0f;
    public float completionDistanceWaypoint = 0.5f;

    public float acceleration = 1.0f;

    private Pawn pawn = null;
    private Seeker seeker = null;
    private Queue<Vector3> vectorPath = new Queue<Vector3>();

    private Vector3? currentTarget = null;

    private float currentAcceleration = 0.0f;

    private Vector3 oldPosition = Vector3.zero;
    private bool computingPath = false;

    protected override string OnInit()
    {
        pawn = agent.GetComponent<Pawn>();
        seeker = agent.GetComponent<Seeker>();

        return base.OnInit();
    }

    protected override async void OnExecute()
    {
        if (targetTransform.value == null)
            targetTransform.value = LevelManager.Instance.PlayerInput.transform;

        bool success = await RecomputePath();
        if (!success)
            oldPosition = Vector3.positiveInfinity;

        currentAcceleration = 0.0f;

        MonoManager.current.onFixedUpdate += OnFixedUpdate;
    }

    protected override void OnStop()
    {
        MonoManager.current.onFixedUpdate -= OnFixedUpdate;
    }

    private void OnFixedUpdate()
    {
        if (Vector3.Distance(targetTransform.value.position, oldPosition) > recomputeDistance && !computingPath)
            RecomputePath();

        if (currentTarget == null)
        {
            if (vectorPath.Count == 0)
                vectorPath.Enqueue(targetTransform.value.position);

            currentTarget = vectorPath.Dequeue();
        }
        float distance = Vector3.Distance(pawn.transform.position, currentTarget.Value);

        Vector3 direction = Vector3.ProjectOnPlane(currentTarget.Value - pawn.transform.position, Vector3.up).normalized * currentAcceleration;

        pawn.Move(direction.x, direction.z);
        pawn.LookTo(new Vector2(direction.x, direction.z));

        if (distance <= completionDistanceWaypoint)
            currentTarget = null;

        currentAcceleration = Mathf.Clamp01(currentAcceleration + Time.fixedDeltaTime * acceleration);
    }

    private async Task<bool> RecomputePath()
    {
        computingPath = true;
        ABPath path = ABPath.Construct(pawn.transform.position, targetTransform.value.position);
        await seeker.StartPath(path);

        computingPath = false;

        if (path.error)
            return false;

        vectorPath = new Queue<Vector3>(path.vectorPath);
        if (vectorPath.Count > 0)
            vectorPath.Dequeue();
        currentTarget = null;

        oldPosition = targetTransform.value.position;

        return true;
    }
}