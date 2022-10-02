using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DialogueCharacterManager : MonoBehaviour
{
    private Image image = null;

    public DialogueCharacterData CurrentCharacter { get; private set; } = null;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetCharacter(DialogueCharacterData character)
    {
        if (character != null)
        {
            image.sprite = character.idleSprite;
            CurrentCharacter = character;
        }
        else
        {
            image.sprite = null;
            CurrentCharacter = null;
        }
    }

    public void SetEmotion(string emotion)
    {
        if (CurrentCharacter == null)
            return;

        if (CurrentCharacter.emotions.ContainsKey(emotion))
            image.sprite = CurrentCharacter.emotions[emotion];
        else
        {
            if (!string.Equals(emotion, "Idle"))
                Debug.LogError($"Couldn't find emotion {emotion} for character {CurrentCharacter.name}. Set Idle sprite.");
            image.sprite = CurrentCharacter.idleSprite;
        }
    }

    public void ShakeCharacter(float force = 1.0f)
    {
        RectTransform transform = image.transform as RectTransform;

        float yPos = transform.anchoredPosition.y;
        transform.DOShakeAnchorPos(0.5f, force * 100.0f, 100)
            .SetUpdate(UpdateType.Late)
            .OnUpdate(() => 
            {
                Vector2 v = transform.anchoredPosition;
                v.y = yPos;
                transform.anchoredPosition = v;
            });
    }
}
