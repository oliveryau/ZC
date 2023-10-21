using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUi : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private SceneTransition sceneTransition;

    public bool IsOpen { get; private set; }

    private DialogueTypewriter dialogueTypewriter;
    private ScenesManager scenesManager;

    private void Start()
    {
        dialogueTypewriter = GetComponent<DialogueTypewriter>();
        scenesManager = FindObjectOfType<ScenesManager>();

        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach (DialogueLine dialogueLine in dialogueObject.DialogueLines)
        {
            speakerText.text = dialogueLine.speaker;
            yield return RunTypingEffect(dialogueLine.dialogue);
            dialogueText.text = dialogueLine.dialogue;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0));

            speakerText.text = null;
        }

        CloseDialogueBox(dialogueObject.id);
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        dialogueTypewriter.Run(dialogue, dialogueText);

        while (dialogueTypewriter.isRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
            {
                dialogueTypewriter.Stop();
            }
        }
    }

    private void CloseDialogueBox(int id = 0)
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        dialogueText.text = null;

        if (id == 2)
        {
            sceneTransition.prevPosition = GameObject.FindWithTag("Player").transform.position;
            StartCoroutine(scenesManager.LoadLevelFromMap(id));
        }
        else if (id == 3)
        {
            sceneTransition.prevPosition = GameObject.FindWithTag("Player").transform.position;
            StartCoroutine(scenesManager.LoadLevelFromMap(id));
        }
    }
}
