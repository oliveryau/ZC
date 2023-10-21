using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact(PlayerMovement playerMovement);
}

public class DialogueActivator : MonoBehaviour, IInteractable
{
    public DialogueObject dialogueObject;
    [SerializeField] private GameObject canSpeakIndicator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.Interactable = this;
            canSpeakIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMovement playerMovement))
        {
            if (playerMovement.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                playerMovement.Interactable = null;
                canSpeakIndicator.SetActive(false);
            }
        }
    }

    public void Interact(PlayerMovement playerMovement)
    {
        playerMovement.DialogueUi.ShowDialogue(dialogueObject);
    }
}
