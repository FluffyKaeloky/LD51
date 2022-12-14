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
            Debug.Log("Game state changed : " + currentGameState);
        }
    }
    private GameStates currentGameState = GameStates.Intro;

    private int currentLevelIndex = -1;

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

    private void Start()
    {
        if (screenFader != null)
            screenFader.FadeOut();
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

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneInternal(int sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(loadingSceneIndex);

        while (op.progress < 1.0f)
            yield return null;

        //SceneManager.LoadScene(loadingSceneIndex);

        screenFader.SnapTo(0.0f);

        yield return null;

        op = SceneManager.LoadSceneAsync(sceneIndex);
        op.allowSceneActivation = false;

        yield return new WaitForSeconds(3.0f);

        while (op.progress < 0.9f)
            yield return null;

        op.allowSceneActivation = true;


        while (op.progress < 1.0f)
            yield return null;

        screenFader.SnapTo(1.0f);

        screenFader.FadeOut(() => CurrentGameState = GameStates.Intro);
    }

    public void GameOver()
    {
        CurrentGameState = GameStates.Loading;

        screenFader.FadeIn(() =>
        {
            StartCoroutine(LoadSceneInternal(sceneIndexes[currentLevelIndex]));
        });
    }
}
