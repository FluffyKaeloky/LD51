using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLoose : MonoBehaviour
{
    private AlertnessManager alertnessManager = null;

    private void Start()
    {
        alertnessManager = GetComponent<AlertnessManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        Entity entity = other.gameObject.GetComponentInParent<Entity>();

        if (entity != null && string.Compare(entity.name,"Player") == 0)
        {
            if (alertnessManager.AlertState == AlertnessManager.AlertStates.Chasing)
                LevelManager.Instance.Loose();
            else if (alertnessManager.AlertState == AlertnessManager.AlertStates.NoAlert)
                alertnessManager.BecomeAlerted(other.transform.position);
        }    
    }

}
