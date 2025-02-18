using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pcontroller : MonoBehaviour
{


    public float moveSpeed = 5f;
    private float horizontalInput;
    private Rigidbody rb;

    public float jumpForce = 3f;


    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        GetInput();
        MovePlayer();

    }
    void Update()
    {
        Jump();

    }
    void GetInput()
    {
        Debug.Log("getting input");

        horizontalInput = Input.GetAxis("Horizontal"); // Rotation
    }

    void MovePlayer()
    {
        Debug.Log("working");
        Vector3 moveDirection = new Vector3(0, 0, horizontalInput).normalized;
        // transform.forward * verticalInput * moveSpeed * Time.deltaTime;move
        moveDirection.Normalize();
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        // rb.MovePosition(rb.position + moveDirection);

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Prevents multiple jumps until landing
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Player can jump again
        }
    }
}

//using System;
//using System.Collections;
//using UnityEngine;

//public class Pcontroller : MonoBehaviour
//{


//    [Header("Movement Settings")]
//    public float moveSpeed = 5f;
//    public float acceleration = 10f;
//    public float deceleration = 10f;
//    public float airControl = 0.5f;

//    [Header("Jump Settings")]
//    public float jumpForce = 10f;
//    public float jumpBufferTime = 0.2f;
//    public float coyoteTime = 0.2f;
//    public float gravityMultiplier = 2f;
//    public float lowJumpMultiplier = 2.5f;

//    [Header("Collision Settings")]
//    public LayerMask groundLayer;
//    public Transform groundCheck;
//    public float groundCheckRadius = 0.3f;

//    private Rigidbody rb;
//    private CapsuleCollider col;
//    private float inputDir;
//    private bool isGrounded;
//    private bool isJumping;
//    private bool jumpBuffer;
//    private float jumpBufferCounter;
//    private float coyoteTimeCounter;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//        col = GetComponent<CapsuleCollider>();
//        rb.freezeRotation = true; // Prevent unwanted rotation
//    }

//    private void Update()
//    {
//        // Get Input (Now only on Z-axis)
//        inputDir = Input.GetAxisRaw("Horizontal");

//        // Ground Check
//        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);


//        Jump();
//    }

//    private void FixedUpdate()
//    {
//        Move();
//        ApplyGravity();
//    }

//    private void Move()
//    {
//        // Only Move along the Z-axis based on horizontal input
//        Vector3 moveVector = transform.forward * inputDir; // Z-axis movement

//        float targetSpeed = moveSpeed * Mathf.Abs(inputDir);

//        if (isGrounded)
//        {
//            rb.velocity = new Vector3(
//                rb.velocity.x, // No movement on X-axis
//                rb.velocity.y, // Keep the Y velocity (for jumping/falling)
//                Mathf.Lerp(rb.velocity.z, moveVector.z * targetSpeed, acceleration * Time.fixedDeltaTime)
//            );
//        }
//        else
//        {
//            rb.velocity = new Vector3(
//                rb.velocity.x, // No movement on X-axis
//                rb.velocity.y, // Keep the Y velocity (for jumping/falling)
//                Mathf.Lerp(rb.velocity.z, moveVector.z * targetSpeed * airControl, deceleration * Time.fixedDeltaTime)
//            );
//        }
//    }

//    void Jump()
//    {
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//            isGrounded = false; // Prevents multiple jumps until landing
//        }
//    }


//    private void ApplyGravity()
//    {
//        if (!isGrounded)
//        {
//            rb.velocity += Vector3.down * gravityMultiplier * Time.fixedDeltaTime;
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        if (groundCheck != null)
//        {
//            Gizmos.color = Color.red;
//            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
//        }
//    }
//}

