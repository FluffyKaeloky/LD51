using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "DialoguePacedChars", menuName = "RGWars/Dialogue/Create DialoguePacedChars")]
public class DialoguePacedChars : SerializedScriptableObject
{
    public Dictionary<char, float> pacedChars = new Dictionary<char, float>();

    #region Class Declarations

    public class ParsedDialogueString
    {
        public float Duration { get; private set; } = 0.0f;
        public string Str { get; private set; } = "";

        private Dictionary<char, float> pacedChars = new Dictionary<char, float>();
        private float typeSpeed = 30.0f;

        public ParsedDialogueString(string str, Dictionary<char, float> pacedChars, float typeSpeed)
        {
            Str = str;
            this.pacedChars = pacedChars;
            this.typeSpeed = typeSpeed;
            Duration = EstimateDutation();
        }

        public string Evaluate(float completion)
        {
            float targetTime = completion * Duration;

            string evaluation = "";

            float currentTime = 0.0f;
            foreach (char c in Str)
            {
                if (currentTime > targetTime)
                    return evaluation;

                if (pacedChars.ContainsKey(c))
                    currentTime += pacedChars[c];
                else
                    currentTime += 1.0f / typeSpeed;

                evaluation += c;
            }

            return Str;
        }

        private float EstimateDutation()
        {
            float duration = 0.0f;

            foreach (char c in Str)
            {
                if (pacedChars.ContainsKey(c))
                    duration += pacedChars[c];
                else
                    duration += 1.0f / typeSpeed;
            }

            return duration;
        }
    }

    #endregion
}
