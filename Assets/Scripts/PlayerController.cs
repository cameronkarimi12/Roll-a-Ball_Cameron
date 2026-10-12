/************************************************************
* COMPONENT OF: Player
* REQUIRED DEPENDENCIES: Rigidbody Component, Camera Reference
* DESCRIPTION: Listens for player movement and jump inputs to apply physical forces to the Rigidbody component aligned relative to the main camera's orientation.
* AUTHOR: CKarimi
* VERSION: 1.2
* VERSION 1.2: Align WASD player movement relative to the current camera view orientation.
*************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movement fields
    private float horizontalMovement;
    private float verticalMovement;

    [Tooltip("Movement force applied to the Rigidbody for speed tuning.")]
    [SerializeField] private float force = 8.0f; // Increased default speed for faster movement

    [Tooltip("Upward impulse force applied to the Rigidbody when jumping.")]
    [SerializeField] private float jumpForce = 10.0f; // Increased default force for higher jump height

    [Tooltip("Reference to the main camera transform to align movement relative to camera view. If unassigned, automatically finds Main Camera.")]
    [SerializeField] private Transform cameraTransform;

    private bool jumpRequested;
    private bool isGrounded;

    private Rigidbody rb;

    private void Awake()
    {
        // Cache the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Finds and caches the main camera transform if unassigned in the Inspector
        FindCameraIfNull();
    }

    private void Update()
    {
        // Delegates input checking and camera reference checks to maintain single responsibility in Update
        ProcessKeyboardInput();
    }

    // Process keyboard input specifically for jump key presses
    private void ProcessKeyboardInput()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }
    }

    // FixedUpdate is called at fixed intervals for physics processing
    void FixedUpdate()
    {
        CheckGrounded();
        MovePlayer();
        TryJump();
    }

    // Checks if camera reference is assigned; if null, searches for Main Camera transform
    private void FindCameraIfNull()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Applies physical directional movement force relative to the camera view direction
    private void MovePlayer()
    {
        // Ensure camera transform reference exists
        FindCameraIfNull();

        Vector3 moveDirection;

        if (cameraTransform != null)
        {
            // Extract camera forward and right directions flattened on the horizontal XZ plane
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            // Calculate movement vector aligned relative to the camera orientation
            moveDirection = (camForward * verticalMovement) + (camRight * horizontalMovement);
        }
        else
        {
            // Fallback to absolute world directions if camera is missing
            moveDirection = new Vector3(horizontalMovement, 0f, verticalMovement);
        }

        // Apply calculated movement force to the Rigidbody
        rb.AddForce(moveDirection * force);
    }

    // Applies upward impulse force to perform a jump if grounded and requested
    private void TryJump()
    {
        if (!jumpRequested || !isGrounded)
        {
            return;
        }

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        jumpRequested = false;
    }

    // Checks if the player is currently contacting the ground surface using a Raycast
    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.05f);
    }

    // Gets the user key input and uses it to assign movement directions
    private void SetMoveDirection(Vector2 input)
    {
        horizontalMovement = input.x;
        verticalMovement = input.y;
    }

    // Listens for WASD and arrow key input then calls SetMoveDirection
    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        SetMoveDirection(ctx.ReadValue<Vector2>());
    }

    // Listens for jump input action triggers
    public void OnJumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            jumpRequested = true;
        }
    }
}