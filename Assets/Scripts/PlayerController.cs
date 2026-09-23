/************************************************************
* COMPONENT OF: Player
* REQUIRED DEPENDENCIES: Rigidbody Component
* DESCRIPTION: It listens for WASD and Arrow key presses to set 
*              vertical and horizontal directions. It uses those
*              to push the player's Rigidbody in that direction
*              at a preset force.
* AUTHOR: CKarimi
* VERSION: 1.0
*************************************************************/
using UnityEngine; using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // Movement fields
    private float horizontalMovement;
    private float verticalMovement;
    private float force;
    private float jumpForce;
    private bool jumpRequested;
    private bool isGrounded;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        force = 4.75f;
        jumpForce = 6.5f;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckGrounded();
        MovePlayer();
        TryJump();
    }

    private void MovePlayer()
    {
        Vector3 direction = new Vector3(horizontalMovement, 0, verticalMovement);
        rb.AddForce(direction * force);
    }

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

    public void OnJumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            jumpRequested = true;
        }
    }
}
