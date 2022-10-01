using NodeCanvas.Framework;
using ParadoxNotion.Services;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AITurnTowards : ActionTask
{
    public BBParameter<Vector3> target = new BBParameter<Vector3>();

    private Pawn pawn = null;

    private float timer = 1.0f;

    private Vector3 dir = Vector3.zero;

    protected override string OnInit()
    {
        pawn = agent.GetComponent<Pawn>();

        return base.OnInit();
    }

    protected override void OnExecute()
    {
        timer = 1.0f;
        dir = Vector3.ProjectOnPlane(target.value - agent.transform.position, Vector3.up).normalized;
        MonoManager.current.onFixedUpdate += OnFixedUpdate;
    }

    protected override void OnStop()
    {
        MonoManager.current.onFixedUpdate -= OnFixedUpdate;
    }

    private void OnFixedUpdate()
    {
        pawn.LookTo(dir);

        timer -= Time.fixedDeltaTime;

        if (timer <= 0.0f)
            EndAction();
    }
}
