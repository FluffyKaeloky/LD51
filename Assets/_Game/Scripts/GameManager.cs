using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        Intro,
        Game,
        Outro,
        Loading
    }

    [Serializable]
    public class OnGameStateChangedArgs
    {
        public GameStates OldState { get; private set; } = GameStates.Intro;
        public GameStates NewState { get; private set; } = GameStates.Intro;

        public OnGameStateChangedArgs(GameStates oldState, GameStates newState)
        {
            OldState = oldState;
            NewState = newState;
        }
    }

    public static GameManager Instance { get; private set; } = null;

    [Serializable]
    public class OnGameStateChanged : UnityEvent<OnGameStateChangedArgs> { }

    public int loadingSceneIndex = 1;

    public List<int> sceneIndexes = new List<int>();

    public OnGameStateChanged onGameStateChanged = new OnGameStateChanged();

    public GameStates CurrentGameState { get { return currentGameState; } 
        set
        {
            if (currentGameState == value)
                return;

            GameStates oldGameState = currentGameState;
            currentGameState = value;

            onGameStateChanged.Invoke(new OnGameStateChangedArgs(oldGameState, value));
        }
    }
    private GameStates currentGameState = GameStates.Intro;

    private int currentLevelIndex = 0;

    private UIScreenFader screenFader = null;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        screenFader = GetComponentInChildren<UIScreenFader>();
    }

    public void LoadNextLevel()
    {
        CurrentGameState = GameStates.Loading;
        currentLevelIndex++;

        screenFader.FadeIn(() => 
        {
            StartCoroutine(LoadSceneInternal(sceneIndexes[currentLevelIndex]));
        });
    }

    private IEnumerator LoadSceneInternal(int sceneIndex)
    {
        SceneManager.LoadScene(loadingSceneIndex);

        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
            yield return null;

        op.allowSceneActivation = true;

        while (op.progress < 1.0f)
            yield return null;

        screenFader.FadeOut(() => CurrentGameState = GameStates.Intro);
    }
}
