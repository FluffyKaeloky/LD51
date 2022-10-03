using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipIntroState : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.onGameStateChanged.AddListener(OnGameStateChanged);
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGameStateChanged.RemoveListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameManager.OnGameStateChangedArgs args)
    {
        if (args.NewState == GameManager.GameStates.Intro)
            GameManager.Instance.CurrentGameState = GameManager.GameStates.Game;
    }
}
