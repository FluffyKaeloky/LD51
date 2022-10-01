using NodeCanvas.Framework;
using ParadoxNotion.Services;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TurnRandom : ActionTask
{
    private Pawn pawn = null;

    private float timer = 1.0f;

    private Vector2 rng;

    protected override string OnInit()
    {
        pawn = agent.GetComponent<Pawn>();

        return base.OnInit();
    }

    protected override void OnExecute()
    {
        timer = 1.0f;

        rng = new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));

        MonoManager.current.onFixedUpdate += OnFixedUpdate;
    }

    protected override void OnStop()
    {
        MonoManager.current.onFixedUpdate -= OnFixedUpdate;
    }

    private void OnFixedUpdate()
    {
        pawn.LookTo(rng);

        timer -= Time.fixedDeltaTime;

        if (timer <= 0.0f)
            EndAction();
    }
}
