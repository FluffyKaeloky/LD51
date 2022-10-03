using AlmenaraGames;
using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Yarn.Unity;
using static DialoguePacedChars;

[RequireComponent(typeof(Animator))]
public class UIRGDialogueView : DialogueViewBase
{
    public enum CharSide
    {
        None = 0,
        Left = 1,
        Right = 2,
    }

    public static UIRGDialogueView Instance { get; private set; } = null;

    public TMPro.TextMeshProUGUI textView = null;

    public bool waitForInput = true;

    public float printSpeed = 30.0f;

    public AudioObject typingAudioClip = null;

    public string animatorDeployParameterName = "IsDeployed";
    public string highlightParameterName = "HighlightChar";

    public DialogueCharacterDatabase characterDatabase = null;

    public DialogueCharacterManager leftCharacterManager = null;
    public DialogueCharacterManager rightCharacterManager = null;

    public DialoguePacedChars pacedChars = null;

    public List<PlayableDirector> playableDirectors = new List<PlayableDirector>();

    public List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    private Animator animator = null;

    private Tween currentTween = null;

    private bool noWaitNextLine = false;
    private bool waitingForSignal = false;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();

    }

    public override void DialogueStarted()
    {
        animator.SetBool(animatorDeployParameterName, true);
    }

    public override void DialogueComplete()
    {
        animator.SetBool(animatorDeployParameterName, false);
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        ParsedDialogueString parsedDialogue = new ParsedDialogueString(dialogueLine.TextWithoutCharacterName.Text, pacedChars.pacedChars, printSpeed);

        if (leftCharacterManager.CurrentCharacter.name == dialogueLine.CharacterName)
            SetHighlightSide(CharSide.Left);
        else if (rightCharacterManager.CurrentCharacter.name == dialogueLine.CharacterName)
            SetHighlightSide(CharSide.Right);

        int oldCount = 0;
        textView.text = dialogueLine.TextWithoutCharacterName.Text;
        textView.maxVisibleCharacters = 0;
        currentTween = DOTween.To(x =>
        {
            string newContent = parsedDialogue.Evaluate(x);

            if (newContent.Length > oldCount && typingAudioClip != null)
                MultiAudioManager.PlayAudioObject(typingAudioClip, Vector3.zero);

            oldCount = newContent.Length;

            textView.maxVisibleCharacters = newContent.Length;
        }, 0.0f, 1.0f, parsedDialogue.Duration)
            .SetEase(Ease.Linear)
            .SetAutoKill(false);

        if (noWaitNextLine)
            currentTween.OnComplete(() => 
            {
                noWaitNextLine = false;
                onDialogueLineFinished.Invoke();
            });
    }

    public override void DismissLine(Action onDismissalComplete)
    {
        if (!noWaitNextLine)
        {
            textView.text = "";
            onDismissalComplete?.Invoke();
        }
    }

    public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        if (currentTween != null && !currentTween.IsComplete())
        {
            currentTween.Complete();
            currentTween.Kill();
            currentTween = null;
        }
        else
            onDialogueLineFinished?.Invoke();
    }

    public override void UserRequestedViewAdvancement()
    {
        if (noWaitNextLine)
            return;

        requestInterrupt.Invoke();
    }

    [YarnCommand("l_character")]
    private static void SetCharacterLeft(string name) =>
        SetCharacter(true, name);

    [YarnCommand("r_character")]
    private static void SetCharacterRight(string name) =>
        SetCharacter(false, name);

    [YarnCommand("r_character_come_in")]
    private static void SetCharacterRightComeIn() =>
        Instance.SetCharacterComeIn(false, true);

    [YarnCommand("r_character_come_out")]
    private static void SetCharacterRightComeOut() =>
        Instance.SetCharacterComeIn(false, false);

    [YarnCommand("l_character_come_in")]
    private static void SetCharacterLeftComeIn() =>
        Instance.SetCharacterComeIn(true, true);

    [YarnCommand("l_character_come_out")]
    private static void SetCharacterLeftComeOut() =>
        Instance.SetCharacterComeIn(true, false);

    [YarnCommand("r_set_emotion")]
    private static void SetRightCharacterEmotion(string emotion) =>
        Instance.rightCharacterManager.SetEmotion(emotion);

    [YarnCommand("l_set_emotion")]
    private static void SetLeftCharacterEmotion(string emotion) =>
        Instance.leftCharacterManager.SetEmotion(emotion);

    [YarnCommand("no_wait")]
    private static void DontWaitNextLine() =>
        Instance.noWaitNextLine = true;

    [YarnCommand("play_timeline")]
    private static void PlayTimeline(int index)
    {
        PlayableDirector director = Instance.playableDirectors[index];
        director.Play();
    }

    [YarnCommand("wait_signal")]
    private static IEnumerator WaitSignal()
    {
        Instance.waitingForSignal = true;
        yield return new WaitUntil(() => !Instance.waitingForSignal);
    }

    [YarnCommand("switch_camera")]
    private static void SwitchCamera(int cameraIndex)
    {
        CinemachineVirtualCamera vcam;
        if (cameraIndex < 0)
            vcam = LevelManager.Instance.mainCamera;
        else
            vcam = Instance.cameras[cameraIndex];

        Instance.cameras.ForEach(x => x.Priority = 0);

        vcam.Priority = 99;
    }

    [YarnCommand("continue_timeline")]
    private static void ContinueTimeline(int index)
    {
        PlayableDirector director = Instance.playableDirectors[index];
        //director.Resume();
        director.playableGraph.GetRootPlayable(0).SetSpeed(1.0f);
    }

    public void ContinueSignal()
    {
        waitingForSignal = false;
        //requestInterrupt.Invoke();
    }

    [YarnCommand("l_shake")]
    private static void LeftCharShake(float force = 1.0f) =>
        Instance.leftCharacterManager.ShakeCharacter(force);

    [YarnCommand("r_shake")]
    private static void RightCharShake(float force = 1.0f) =>
        Instance.rightCharacterManager.ShakeCharacter(force);

    private static void SetCharacter(bool leftSide, string name)
    {
        DialogueCharacterManager characterManager = leftSide ? Instance.leftCharacterManager : Instance.rightCharacterManager;

        DialogueCharacterData character = Instance.characterDatabase.characters.Find(x => x.name == name);

        if (character == null)
        {
            Debug.LogError($"Character {name} wasn't found in character database.");
            return;
        }

        characterManager.SetCharacter(character);
    }

    private void SetCharacterComeIn(bool leftSide, bool comeIn)
    {
        string side = leftSide ? "Left" : "Right";

        animator.SetBool($"Character{side}ComeIn", comeIn);
    }

    public void SetHighlightSide(CharSide side)
    {
        if (Application.isPlaying)
            animator.SetInteger(highlightParameterName, (int)side);
    }
}
