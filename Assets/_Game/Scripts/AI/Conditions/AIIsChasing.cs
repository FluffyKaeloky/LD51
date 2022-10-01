using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;
using System.Linq;

public class AIIsChasing : ConditionTask
{
    private AlertnessManager alertnessManager = null;

    protected override string OnInit()
    {
        alertnessManager = agent.GetComponent<AlertnessManager>();
    
        return base.OnInit();
    }

    protected override bool OnCheck()
    {
        if (alertnessManager.AlertState == AlertnessManager.AlertStates.Chasing)
            return true;
        return false;
    }
}
