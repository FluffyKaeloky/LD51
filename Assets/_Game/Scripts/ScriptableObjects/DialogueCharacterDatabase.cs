using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueCharacterDatabase", menuName = "RGWars/DialogueCharacters/Create database")]
public class DialogueCharacterDatabase : ScriptableObject
{
    public List<DialogueCharacterData> characters = new List<DialogueCharacterData>();
}
