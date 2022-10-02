using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLoose : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.gameObject.GetComponentInParent<Entity>();

        if (string.Compare(entity.name,"Player") == 0)
        {
            LevelManager.Instance.Loose();
        }    
    }

}
