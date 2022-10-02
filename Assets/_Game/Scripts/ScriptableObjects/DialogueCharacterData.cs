using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueCharacter", menuName = "RGWars/DialogueCharacters/Create")]
public class DialogueCharacterData : SerializedScriptableObject
{
    public new string name = "NewCharacter";

    public Sprite idleSprite = null;

    public Dictionary<string, Sprite> emotions = new Dictionary<string, Sprite>();
}
