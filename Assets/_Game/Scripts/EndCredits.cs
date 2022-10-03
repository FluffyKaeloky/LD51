using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCredits : MonoBehaviour
{
    public CanvasGroup canvasGroup = null;

    public async void OnDialogueEnd()
    {
        canvasGroup.DOFade(1.0f, 4.0f);

        await new WaitForSeconds(20.0f);

        Application.Quit();
    }
}
