using NodeCanvas.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AIResetAlertState : ActionTask
{
    private AlertnessManager alertnessManager = null;

    protected override string OnInit()
    {
        alertnessManager = agent.GetComponent<AlertnessManager>();

        return base.OnInit();
    }

    protected override void OnExecute()
    {
        alertnessManager.ResetAlertState();
        EndAction();
    }
}
