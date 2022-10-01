using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AlertnessManager))]
public class FOVAlertnessFeedback : MonoBehaviour
{
    public Color alertingColor = Color.red * 0.8f;

    public Color alertedColor = Color.red;

    public new MeshRenderer renderer = null;

    private Material material = null;

    private Color baseColor = Color.white;

    private AlertnessManager alertnessManager = null;

    private void Awake()
    {
        alertnessManager = GetComponent<AlertnessManager>();
    }

    private void Start()
    {
        material = Instantiate(renderer.material);
        renderer.material = material;

        baseColor = material.color;
    }

    private void Update()
    {
        SetAlertness(alertnessManager.Alertness);
    }

    public void SetAlertness(float alertness)
    {
        if (alertnessManager.AlertState == AlertnessManager.AlertStates.NoAlert)
            material.color = Color.Lerp(baseColor, alertingColor, alertness);
        else
            material.DOColor(alertedColor, 0.5f);
    }
}
