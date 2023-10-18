using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float inputHorizontal;
    private float inputVertical;

    [SerializeField] private float moveSpeed;

    [Header("Others")]
    [SerializeField] private DialogueUi dialogueUi;

    public DialogueUi DialogueUi => dialogueUi;
    public IInteractable Interactable { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (dialogueUi.IsOpen) return;

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        #region Dialogue
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Interactable != null)
            {
                Interactable.Interact(this);
            }
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (inputHorizontal != 0 || inputVertical != 0)
        {
            rb.velocity = new Vector2(inputHorizontal, inputVertical).normalized * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (inputHorizontal > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (inputHorizontal < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
