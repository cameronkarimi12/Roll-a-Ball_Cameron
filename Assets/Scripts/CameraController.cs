/************************************************************

COMPONENT OF: main camera

REQUIRED DEPENDENCIES:

DESCRIPTION: Controls the third-person camera to follow behind the player transform by default, while allowing orbit rotation around the player when holding Right Click and dragging the mouse.

AUTHOR: CKarimi

DATE WRITTEN: 2026-09-18

VERSION: 1.5

VERSION 1.5: Change rotation control to Right Click + mouse drag instead of Right Click + WASD.
*************************************************************/

using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
[Header("Player")]
[Tooltip("Drag the Player GameObject transform here. If unassigned, the script will attempt to locate a GameObject tagged 'Player'.")]
[SerializeField] private Transform player;

[Header("Camera Positioning")]
[Tooltip("Distance behind the player transform in world units.")]
[SerializeField] private float distance = 5f;

[Tooltip("Height offset above the player transform in world units.")]
[SerializeField] private float height = 2f;

[Header("Mouse Rotation Settings")]
[Tooltip("Sensitivity multiplier for mouse drag rotation.")]
[SerializeField] private float mouseSensitivity = 0.1f; // Sensitivity applied to mouse delta movement

[Header("Vertical Rotation")]
[SerializeField] private float minPitch = -30f;
[SerializeField] private float maxPitch = 70f;

private float yaw;
private float pitch = 15f;

private void Start()
{
    LockMouse();
    
    // Ensures player reference is acquired at startup if missing in the Inspector
    FindPlayerIfNull();

    // Initialize yaw rotation to match player's initial facing direction
    InitializeYaw();
}

private void Update()
{
    // Delegates input handling for cursor state lock/unlock to keep Update clean
    HandleCursorLockInput();
}

private void LateUpdate()
{
    // Attempts to find player if reference was lost or missing
    FindPlayerIfNull();

    // If player reference is still missing, exit early to prevent errors
    if (player == null)
        return;

    // Processes camera orbit input when holding Right Click + dragging the mouse
    HandleCameraRotationInput();

    // Updates camera positioning and orientation based on calculated pitch and yaw
    UpdateCameraTransform();
}

// Checks if player reference is assigned; if null, searches the scene for GameObject tagged 'Player'
private void FindPlayerIfNull()
{
    if (player == null)
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
}

// Sets initial yaw value to match player's starting forward direction
private void InitializeYaw()
{
    if (player != null)
    {
        yaw = player.eulerAngles.y;
    }
}

// Handles locking and unlocking mouse cursor state based on keyboard and mouse button clicks
private void HandleCursorLockInput()
{
    // ESC unlocks mouse
    if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
    {
        UnlockMouse();
    }

    // Left click locks it again
    if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
    {
        LockMouse();
    }
}

// Reads Right Click + mouse drag input to adjust camera yaw and pitch values
private void HandleCameraRotationInput()
{
    if (Mouse.current == null)
        return;

    // Check if Right Mouse Button is held down
    bool isRightClickPressed = Mouse.current.rightButton.isPressed;

    if (isRightClickPressed)
    {
        // Reads the mouse movement delta vector for this frame
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Calculate rotation changes scaled by mouse sensitivity
        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;

        // Clamp vertical pitch to stay within maximum and minimum look angles
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }
}

// Calculates and updates position and orientation relative to player using pitch and yaw
private void UpdateCameraTransform()
{
    // Construct target rotation quaternion from orbit pitch and yaw values
    Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);

    // Compute offset behind player target pivot using orbital rotation
    Vector3 offset = targetRotation * new Vector3(0f, 0f, -distance);

    // Set camera position using player position, height offset, and rotation offset
    transform.position = player.position + Vector3.up * height + offset;

    // Ensure camera continuously faces the player target pivot
    transform.LookAt(player.position + Vector3.up * height);
}

private void LockMouse()
{
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}

private void UnlockMouse()
{
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}
}