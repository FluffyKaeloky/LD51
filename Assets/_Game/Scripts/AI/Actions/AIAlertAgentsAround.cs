using NodeCanvas.Framework;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AIAlertAgentsAround : ActionTask
{
    public BBParameter<float> radius = 20.0f;

    protected override void OnExecute()
    {
        Physics.OverlapSphere(agent.transform.position, radius.value)
            .Select(x => x.GetComponentInParent<AlertnessManager>())
            .Distinct()
            .NotNull()
            .ForEach(x => x.BecomeAlerted(agent.GetComponent<AlertnessManager>().LastKnownPlayerLocation));

        EndAction();
    }
}
