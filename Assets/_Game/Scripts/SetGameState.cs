using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameState : MonoBehaviour
{
    public GameManager.GameStates newState = GameManager.GameStates.Game;

    public bool triggerOnStart = false;

    private void Start()
    {
        if (triggerOnStart)
            Trigger();
    }

    public void Trigger()
    {
        GameManager.Instance.CurrentGameState = newState;
    }
}
