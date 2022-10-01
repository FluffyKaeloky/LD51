using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FartShockwave : MonoBehaviour
{
    [Serializable]
    public class OnEntityHitArgs 
    {
        public Entity Entity { get; private set; } = null;

        public OnEntityHitArgs(Entity entity)
        {
            Entity = entity;
        }
    }
    [Serializable]
    public class OnEntityHit : UnityEvent<OnEntityHitArgs> { }

    public OnEntityHit onEntityHit = new OnEntityHit();
    public UnityEvent onDestroy = new UnityEvent();

    private List<Entity> hitEntities = new List<Entity>();

    public float force = 1.0f;
    public float timePerForceFactor = 1.0f;

    public float innerScaleDistanceMultipler = 0.80f;

    public Material shockwaveMaterial = null;

    public AnimationCurve cameraShakeByForce = new AnimationCurve();

    private float baseValue = 0.0f;

    private void Start()
    {
        baseValue = shockwaveMaterial.color.a;

        CameraShake.Instance.Shake(0.5f, cameraShakeByForce.Evaluate(force));

        DOTween.To(x =>
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                child.localScale = new Vector3(
                    x * force * i * innerScaleDistanceMultipler,
                    1.0f,
                    x * force * i * innerScaleDistanceMultipler);
            }

            Color c = shockwaveMaterial.color;
            c.a = baseValue * (1.0f - x);
            shockwaveMaterial.color = c;

            List<Entity> newHits = Physics.OverlapSphere(transform.position, x * force)
                .Select(x => x.GetComponentInParent<Entity>())
                .Distinct()
                .NotNull()
                .Except(hitEntities)
                .ToList();

            newHits.ForEach(x => onEntityHit?.Invoke(new OnEntityHitArgs(x)));
            hitEntities.AddRange(newHits);

        }, 0.0f, 1.0f, force * timePerForceFactor)
            .OnComplete(() => 
            {
                onDestroy?.Invoke();
                Destroy(gameObject);
                return;
            })
            .SetEase(Ease.OutExpo);
    }

    private void OnDestroy()
    {
        Color c = shockwaveMaterial.color;
        c.a = baseValue;
        shockwaveMaterial.color = c;
    }
}
