using NodeCanvas.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GetPlayerLocation : ActionTask
{
    public BBParameter<Vector3> outPos = new BBParameter<Vector3>();

    protected override void OnExecute()
    {
        outPos.value = LevelManager.Instance.PlayerInput.transform.position;
        EndAction();
    }
}
