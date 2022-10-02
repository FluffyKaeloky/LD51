using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class FartManager : MonoBehaviour
{
    [SerializeField] private Slider fartSlider;
    [SerializeField] private Image sliderFillImage;
    [SerializeField] private Color lerpedColor, startColor, endColor;

    [SerializeField] private int refrainFartValue;

    private float loadFartSpeed;
    private float fartWillValue;

    [SerializeField] private float willValueSpeed;
    [SerializeField] private AnimationCurve fartWillCurve;
    [SerializeField] private float breakGlassValue;
    [SerializeField] private LayerMask glassMask;

    private float noFartTime;
    public FartShockwave fartShockwavePrefab;
    public UnityEvent onFart = new UnityEvent();
    public UnityEvent onHoldIt = new UnityEvent();



    private void Start()
    {
        sliderFillImage.color = startColor;
        noFartTime = Time.time + 5;
    }


    private void Update()
    {

        if (noFartTime <= Time.time)
        {
            fartWillValue += Time.deltaTime * willValueSpeed;
            lerpedColor = Color.Lerp(startColor, endColor, fartWillValue);
            sliderFillImage.color = lerpedColor;
        }

        if (fartSlider != null)
        {
            fartSlider.value = Mathf.MoveTowards(fartSlider.value, fartSlider.maxValue, loadFartSpeed * Time.deltaTime);

        }
        loadFartSpeed = Mathf.Lerp(1, 10, fartWillCurve.Evaluate(fartWillValue));

        if (fartSlider.value == fartSlider.maxValue && fartSlider != null)
        {
            Fart();
        }
    }

    public void RefrainFart()
    {
        if (fartSlider != null)
        {
            fartSlider.value -= refrainFartValue;
            onHoldIt?.Invoke();
        }
    
    }


    public void Fart()
    {
        print("farted" + fartWillValue);
        float fartPower = fartWillValue;

        fartSlider.gameObject.transform.DOShakePosition(0.8f,new Vector3(1,1,0),50,90,false, true,ShakeRandomnessMode.Full);
        fartSlider.gameObject.transform.DOShakeScale(0.6f, new Vector3(0.1f, 0.1f, 0), 20, 90, true, ShakeRandomnessMode.Full);

        FartShockwave instance = Instantiate(fartShockwavePrefab, transform.position, Quaternion.identity);
        instance.force = fartWillValue * 5;
        instance.onEntityHit.AddListener((x) => 
        {
            Debug.Log("entredansleif");

            if (fartPower >= breakGlassValue)
            {
                x.Entity.GetComponent<GlassObject>()?.BreakGlass(fartPower,transform.position);
            }
        });

        /*//BreakGlass
        if (fartWillValue >= breakGlassValue)
        {
            Collider[] aroundGlasses = Physics.OverlapSphere(transform.position, fartWillValue * 2f, glassMask);
            foreach (var glass in aroundGlasses)
            {
                glass.GetComponent<GlassObject>().BreakGlass();
            }
        }*/

        onFart?.Invoke();


        //resetFart
        fartSlider.value = 0;
        fartWillValue = 0;
        loadFartSpeed = 1;
        noFartTime = Time.time + 5;

        lerpedColor = startColor;
        sliderFillImage.color = startColor;
    }


    //if value Needed
    public void RefrainFart(int value)
    {
        if (fartSlider != null)
        {
            fartSlider.value -= value;
        }
    } 


}
