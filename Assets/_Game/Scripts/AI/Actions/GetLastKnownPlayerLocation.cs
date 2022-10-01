using NodeCanvas.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GetLastKnownPlayerLocation : ActionTask
{
    public BBParameter<Vector3> outPos = new BBParameter<Vector3>();

    private AlertnessManager alertnessManager = null;

    protected override string OnInit()
    {
        alertnessManager = agent.GetComponent<AlertnessManager>();

        return base.OnInit();
    }

    protected override void OnExecute()
    {
        outPos.value = alertnessManager.LastKnownPlayerLocation;
        EndAction();
    }
}
