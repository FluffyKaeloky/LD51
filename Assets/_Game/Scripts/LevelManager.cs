using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } = null;

    public UnityEvent onLoss = new UnityEvent();

    public PlayerInput PlayerInput { get => playerInput; }
    [SerializeField]
    private PlayerInput playerInput = null;

    public CinemachineVirtualCamera mainCamera = null;

    private void Awake()
    {
        Instance = this;
    }

    public void Win()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public async void Loose()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameStates.Game)
            return;

        onLoss?.Invoke();
        GameManager.Instance.CurrentGameState = GameManager.GameStates.Outro;

        await new WaitForSeconds(1.0f);

        GameManager.Instance.GameOver();
    }
}
