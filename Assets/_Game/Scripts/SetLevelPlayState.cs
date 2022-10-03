using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLevelPlayState : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.CurrentGameState = GameManager.GameStates.Game;
    }
}
