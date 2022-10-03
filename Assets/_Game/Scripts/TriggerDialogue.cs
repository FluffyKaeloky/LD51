using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TriggerDialogue : MonoBehaviour
{
    public string dialogueTitle = "DialogueTitle";

    public DialogueRunner dialogueRunner = null;

    private void OnTriggerEnter(Collider other)
    {
        Entity ent = other.GetComponentInParent<Entity>();

        if (ent != null && ent.gameObject.tag == "Player")
            dialogueRunner.StartDialogue(dialogueTitle);
    }
}
