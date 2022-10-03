using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameState : MonoBehaviour
{
    public bool assignOnStart = false;
    public GameManager.GameStates newGameState = GameManager.GameStates.Game;

    private void Start()
    {
        if (!assignOnStart)
            return;

        AssignState();
    }

    public void AssignState()
    {
        GameManager.Instance.CurrentGameState = newGameState;
    }
}
