using NodeCanvas.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AIHasPlayerLOS : ConditionTask
{
    public float loseSightDelay = 2.0f;

    private AlertnessManager alertnessManager = null;

    private float timer = 0.0f;

    protected override string OnInit()
    {
        alertnessManager = agent.GetComponent<AlertnessManager>();

        return base.OnInit();
    }

    protected override bool OnCheck()
    {
        timer -= Time.deltaTime;

        if (alertnessManager.HasPlayerLOS)
            timer = loseSightDelay;

        return timer > 0.0f;
    }
}
