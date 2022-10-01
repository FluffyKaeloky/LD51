using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FartManager : MonoBehaviour
{
    [SerializeField] private Slider fartSlider;

    [SerializeField] private int refrainFartValue;

    private float loadFartSpeed;
    private float fartWillValue;
    [SerializeField] private float willValueSpeed;
    [SerializeField] private AnimationCurve fartWillCurve;


    private float noFartTime;

    public UnityEvent onFart = new UnityEvent();


    private void Start()
    {
        noFartTime = Time.time + 10;
    }


    private void Update()
    {

        if (noFartTime <= Time.time)
        {
            fartWillValue += Time.deltaTime * willValueSpeed;
        }


        fartSlider.value = Mathf.MoveTowards(fartSlider.value, fartSlider.maxValue, loadFartSpeed * Time.deltaTime);
        loadFartSpeed = Mathf.Lerp(1, 10, fartWillCurve.Evaluate(fartWillValue));

        if (fartSlider.value == fartSlider.maxValue)
        {
            Fart();
        }
    }

    public void RefrainFart()
    {
        fartSlider.value -= refrainFartValue;
    }


    public void Fart()
    {
        print("farted");
        onFart?.Invoke();

        //resetFart
        fartSlider.value = 0;
        fartWillValue = 0;
        loadFartSpeed = 1;
        noFartTime = Time.time + 10;
    }


    //if value Needed
    public void RefrainFart(int value)
    {
        fartSlider.value -= value;
    } 


}
