using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    [TextArea(3, 10)] public string dialogue;
}

[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue")]
public class DialogueObject : ScriptableObject 
{
    public int id;
    [SerializeField] private DialogueLine[] dialogueLines;

    public DialogueLine[] DialogueLines => dialogueLines;
}
