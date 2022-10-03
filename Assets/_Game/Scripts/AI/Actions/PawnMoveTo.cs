using NodeCanvas.Framework;
using ParadoxNotion.Services;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

public class PawnMoveTo : ActionTask
{
    public BBParameter<Vector3> target = Vector3.zero;
    public BBParameter<Transform> targetTransform = null;

    public float waypointCompletionDistance = 0.15f;
    public BBParameter<float> completionDistance = 0.15f;

    public bool damping = true;
    public float dampingDistance = 1.0f;

    public float acceleration = 1.0f;

    public BBParameter<bool> alignAtTransformForward = false;

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
        vectorPath.Clear();
        currentTarget = null;

        if (targetTransform != null && targetTransform.value != null)
            target = targetTransform.value.position;

        if (Vector3.Distance(target.value, agent.transform.position) <= completionDistance.value)
        {
            EndAction();
            return;
        }    

        ABPath path = ABPath.Construct(pawn.transform.position, target.value);
        bool isDone = false;
        seeker.StartPath(path, (x) => { isDone = true; });

        await new WaitUntil(() => isDone);
        
        if (path.error || path.CompleteState == PathCompleteState.Partial)
        {
            EndAction(false);
            return;
        }

        vectorPath = new Queue<Vector3>(path.vectorPath);
        /*if (vectorPath.Count > 1)
            vectorPath.Dequeue();*/
        currentAcceleration = Mathf.Clamp01(pawn.Velocity.magnitude / pawn.moveSpeed);

        MonoManager.current.onFixedUpdate += OnFixedUpdate;

        foreach (Vector3 v in vectorPath)
            DebugExtension.DebugWireSphere(v, Color.red, 1.0f, 5.0f);
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
                if (alignAtTransformForward.value)
                    pawn.LookTo(targetTransform.value.forward);

                EndAction();
                return;
            }

            currentTarget = vectorPath.Dequeue();
        }
        float distance = Vector3.Distance(pawn.transform.position, currentTarget.Value);
        float dampingValue = 1.0f;
        float finalCompletionDistance = (vectorPath.Count == 0 ? completionDistance.value : waypointCompletionDistance);

        if (damping && vectorPath.Count == 0 && distance - finalCompletionDistance <= dampingDistance)
            dampingValue = Mathf.Clamp((distance - finalCompletionDistance) / dampingDistance, 0.05f, 1.0f);

        Vector3 direction = Vector3.ProjectOnPlane(currentTarget.Value - pawn.transform.position, Vector3.up).normalized * currentAcceleration * dampingValue;

        pawn.Move(direction.x, direction.z);
        pawn.LookTo(new Vector2(direction.x, direction.z));

        if (distance <= finalCompletionDistance)
            currentTarget = null;

        currentAcceleration = Mathf.Clamp01(currentAcceleration + Time.fixedDeltaTime * acceleration);
    }
}