using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pcontroller : MonoBehaviour
{


    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    private float verticalInput;
    private float horizontalInput;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        GetInput();
        MovePlayer();
        RotatePlayer();
    }

    void GetInput()
    {
        Debug.Log("getting input");
        verticalInput = Input.GetAxis("Vertical"); // Forward and backward movement
        horizontalInput = Input.GetAxis("Horizontal"); // Rotation
    }

    void MovePlayer()
    {
        Debug.Log("working");
        Vector3 moveDirection = transform.forward * verticalInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }

    void RotatePlayer()
    {
        float rotation = horizontalInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }
}


