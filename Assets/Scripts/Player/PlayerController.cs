using UnityEngine;

/// <summary>
/// Core player movement and input controller for 3D games.
/// Handles walking, sprinting, jumping, and ground detection.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 2f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDrag_Jump = 0f;
    [SerializeField] private float airResistance = 0.95f;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDragOnAir = 0.05f;

    private Rigidbody rb;
    private Camera playerCamera;
    
    private float horizontalInput;
    private float verticalInput;
    private bool isGrounded;
    private bool isSprinting;
    private Vector3 moveDirection;
    private float currentSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;

        // Optimize rigidbody
        rb.freezeRotation = true;
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        // Ground check - raycast from center downward
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        HandleInput();
        HandleSpeed();
        HandleDrag();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        // Movement input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Sprint input
        isSprinting = Input.GetKey(KeyCode.LeftShift) && isGrounded;

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void HandleSpeed()
    {
        // Update current speed based on sprint state
        currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
    }

    private void MovePlayer()
    {
        // Calculate movement direction relative to where player is looking
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        // Apply force
        rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);

        // Limit velocity on ground
        if (isGrounded)
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > currentSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * currentSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void HandleDrag()
    {
        // Apply different drag values based on ground state
        rb.drag = isGrounded ? groundDrag : airDrag;
    }

    private void Jump()
    {
        // Reset Y velocity before jumping
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Add jump force
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public bool IsGrounded() => isGrounded;
    public bool IsSprinting() => isSprinting;
    public float GetCurrentSpeed() => currentSpeed;
}
