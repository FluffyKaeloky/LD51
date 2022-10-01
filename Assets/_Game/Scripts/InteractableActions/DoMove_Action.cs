using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoMove_Action : MonoBehaviour
{
    public float time;
    public Vector3 endPos;


    public void doAction()
    {
        transform.DOMove(this.transform.position + endPos, time, false);
    }
}
