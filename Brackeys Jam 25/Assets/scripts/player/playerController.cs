using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{ 
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movement in X direction
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(moveInput * moveSpeed, rb.velocity.y, 0);

        // Checking if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }
}


