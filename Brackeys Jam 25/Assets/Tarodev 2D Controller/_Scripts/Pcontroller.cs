
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Pcontroller : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float jumpForce = 5f;  // Increased jump force
//    public float gravityMultiplier = 10f; // Makes falling faster
//    public float groundCheckDistance = 0.3f;
//    public LayerMask groundLayer;

//    private float horizontalInput;
//    private Rigidbody rb;
//    private bool isGrounded;
//    private bool facingRight = true; // To track player's direction

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.freezeRotation = true; // Prevent unwanted rotation

//    }

//    void Update()
//    {
//        GetInput();
//        Jump();
//    }

//    void FixedUpdate()
//    {
//        MovePlayer();
//    }

//    void GetInput()
//    {
//        horizontalInput = Input.GetAxis("Horizontal");
//    }

//    void MovePlayer()
//    {
//        Vector3 moveDirection = new Vector3(0, 0, horizontalInput).normalized;
//        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, moveDirection.z * moveSpeed);

//        // Change rotation based on movement direction
//        if (horizontalInput > 0 && !facingRight)
//        {
//            FlipCharacter(true);
//        }
//        else if (horizontalInput < 0 && facingRight)
//        {
//            FlipCharacter(false);
//        }
//    }
//    void FlipCharacter(bool faceRight)
//    {
//        facingRight = faceRight;
//        transform.rotation = faceRight ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
//    }
//    void Jump()
//    {
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            Debug.Log("Jumping");
//            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
//            isGrounded = false;
//        }

//        // Apply extra gravity when falling
//        if (!isGrounded && rb.velocity.y < 0)
//        {
//            rb.velocity += Vector3.down * gravityMultiplier * Time.deltaTime;
//        }
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))  // Ensure layer name is correct
//        {
//            isGrounded = true;
//            Debug.Log("Collided with Ground!");
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pcontroller : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 8f;
    public float gravityMultiplier = 2f;
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    public Transform groundCheck;  // ?? Reference to GroundCheck Empty Object

    private float horizontalInput;
    private Rigidbody rb;
    private bool isGrounded;
    private bool facingRight = true;

    private float coyoteTime = 0.1f; // Small buffer time after leaving a platform
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.1f; // Allows jumping slightly before landing
    private float jumpBufferCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        GetInput();
        CheckGround();
        HandleJump();
        FlipCharacter();
    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyExtraGravity();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime; // Buffer jump input
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void MovePlayer()
    {
        float targetVelocity = horizontalInput * moveSpeed;
        Vector3 velocity = rb.velocity;
        velocity.z = targetVelocity;
        rb.velocity = velocity;
    }

    void HandleJump()
    {
        Debug.Log("working");
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Reset coyote time if on the ground
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
            jumpBufferCounter = 0; // Reset buffer after jumping
        }
        // ?? Jump ke baad slight downward pull apply karo for smooth feel
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - 1f, rb.velocity.z);
    }

    //void ApplyExtraGravity()
    //{
    //    if (!isGrounded && rb.velocity.y < 0)
    //    {
    //        rb.velocity += Vector3.down * gravityMultiplier * 9.81f; // ?? More gravity applied instantly
    //    }
    //}
    void ApplyExtraGravity()
    {
        if (!isGrounded && rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down * gravityMultiplier * 20f, ForceMode.Acceleration);
        }
    }

    void FlipCharacter()
    {
        if (horizontalInput > 0 && !facingRight)
        {
            facingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontalInput < 0 && facingRight)
        {
            facingRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void CheckGround()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, out hit, groundCheckDistance, groundLayer);

        Debug.Log("On Ground: " + isGrounded);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }
}
