
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pcontroller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;  // Increased jump force
    public float gravityMultiplier = 10f; // Makes falling faster
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    private float horizontalInput;
    private Rigidbody rb;
    private bool isGrounded;
    private bool facingRight = true; // To track player's direction

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent unwanted rotation

    }

    void Update()
    {
        GetInput();
        Jump();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void MovePlayer()
    {
        Vector3 moveDirection = new Vector3(0, 0, horizontalInput).normalized;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, moveDirection.z * moveSpeed);

        // Change rotation based on movement direction
        if (horizontalInput > 0 && !facingRight)
        {
            FlipCharacter(true);
        }
        else if (horizontalInput < 0 && facingRight)
        {
            FlipCharacter(false);
        }
    }
    void FlipCharacter(bool faceRight)
    {
        facingRight = faceRight;
        transform.rotation = faceRight ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jumping");
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
        }

        // Apply extra gravity when falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            rb.velocity += Vector3.down * gravityMultiplier * Time.deltaTime;
        }
    }
   
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))  // Ensure layer name is correct
        {
            isGrounded = true;
            Debug.Log("Collided with Ground!");
        }
    }
}

