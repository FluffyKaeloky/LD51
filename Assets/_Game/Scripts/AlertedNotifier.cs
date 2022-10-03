using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertedNotifier : MonoBehaviour
{
    public static AlertedNotifier Instance { get; private set; } = null;

    public bool IsBeingChased { get { return chasingManagers.Count > 0; } }

    private List<AlertnessManager> chasingManagers = new List<AlertnessManager>();

    private void Awake()
    {
        Instance = this;
    }

    public void PushChaseAlert(AlertnessManager instigator)
    {
        if (!chasingManagers.Contains(instigator))
        {
            chasingManagers.Add(instigator);
            if (BGMManager.Instance != null)
                BGMManager.Instance.SetMixerState(0.0f, 1.0f, 0.0f, 1.0f);
        }
    }

    public void DropChaseAlert(AlertnessManager instigator)
    {
        if (chasingManagers.Contains(instigator))
        {
            chasingManagers.Remove(instigator);
            if (chasingManagers.Count == 0 && BGMManager.Instance != null)
                BGMManager.Instance.SetMixerState(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }
}
