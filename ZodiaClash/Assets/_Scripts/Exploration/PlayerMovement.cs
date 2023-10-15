using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float inputHorizontal;
    private float inputVertical;

    [SerializeField] private float moveSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
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
    }
}
