/************************************************************

COMPONENT OF: Collectible Prefabs / Trigger Volume

REQUIRED DEPENDENCIES: GameManager Component (Optional), MeshRenderer Component

DESCRIPTION: Automatically finds and opens any target door in the scene matching either its Key Identifier or Material upon collection.

Author: CKarimi

VERSION: 1.4

VERSION 1.4: Add flexible key identifier matching alongside material matching to ensure doors open reliably.
*************************************************************/
using UnityEngine;

public class DoorKeyCollectible : MonoBehaviour
{
[Header("Key Setup")]
[Tooltip("Enter a unique key tag/ID (e.g., 'RedKey', 'Door1') that matches the Door's Key Identifier. Leave blank to use Material matching.")]
[SerializeField] private string keyIdentifier = "";

[Header("Door Movement Settings")]
[Tooltip("The position vector where the door should move to relative to its start position when opened.")]
[SerializeField] private Vector3 openPositionOffset = new Vector3(0f, 4f, 0f);

[Tooltip("Speed at which the door moves towards its open position.")]
[SerializeField] private float doorOpenSpeed = 3f;

[Header("Audio & Visual Feedback")]
[SerializeField] private AudioClip collectSound;
[SerializeField] private GameObject collectParticlePrefab;

private GameManager gameManager;
private bool isCollected = false;
private Transform matchedDoorTransform;
private Vector3 targetDoorEndPosition;

// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
{
    // Cache reference to GameManager if one exists in the scene
    FindGameManager();
}

// Update is called once per frame
void Update()
{
    // Delegates continuous door movement processing to keep Update clean
    ProcessDoorMovement();
}

// Call when this object collides with a trigger
private void OnTriggerEnter(Collider other)
{
    // Check if collision is with Player and item has not been collected yet
    if (other.CompareTag("Player") && !isCollected)
    {
        TryFindAndOpenMatchingDoor();
    }
}

// Finds the Game Manager in the scene safely
private void FindGameManager()
{
    gameManager = FindAnyObjectByType<GameManager>();
}

// Scans all door objects in the scene to find one sharing the exact material or Key Identifier as this collectible
private void TryFindAndOpenMatchingDoor()
{
    MeshRenderer myRenderer = GetComponent<MeshRenderer>();

    // Search the scene for any MeshRenderers to find a matching material or door target
    MeshRenderer[] allRenderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);

    foreach (MeshRenderer candidateRenderer in allRenderers)
    {
        // Skip checking itself
        if (candidateRenderer.gameObject == gameObject)
        {
            continue;
        }

        bool isMatch = CheckIfDoorMatches(myRenderer, candidateRenderer);

        if (isMatch)
        {
            // Match found: store reference and set target open destination
            matchedDoorTransform = candidateRenderer.transform;
            targetDoorEndPosition = matchedDoorTransform.position + openPositionOffset;
            
            HandleCollection();
            break;
        }
    }
}

// Evaluates whether a candidate MeshRenderer matches this collectible by Key Identifier or Material
private bool CheckIfDoorMatches(MeshRenderer myRenderer, MeshRenderer candidateRenderer)
{
    // Check 1: Match by Key Identifier if specified
    if (!string.IsNullOrEmpty(keyIdentifier))
    {
        DoorKeyCollectible doorKeyComp = candidateRenderer.GetComponent<DoorKeyCollectible>();
        if (doorKeyComp != null && doorKeyComp.GetKeyIdentifier() == keyIdentifier)
        {
            return true;
        }
    }

    // Check 2: Match by Material if both renderers have materials assigned
    if (myRenderer != null && myRenderer.sharedMaterial != null && candidateRenderer.sharedMaterial != null)
    {
        string myMatName = myRenderer.sharedMaterial.name.Replace(" (Instance)", "");
        string candidateMatName = candidateRenderer.sharedMaterial.name.Replace(" (Instance)", "");

        if (myMatName == candidateMatName)
        {
            return true;
        }
    }

    return false;
}

// Public getter to allow other components to read this collectible's Key Identifier
public string GetKeyIdentifier()
{
    return keyIdentifier;
}

// Handles pickup logic, spawns effects, updates game state, and initiates door opening
private void HandleCollection()
{
    isCollected = true;

    if (gameManager != null)
    {
        gameManager.UpdateRemaining();
    }

    PlayFeedbackEffects();
    
    // Hide collectible visuals and disable its collider while door finishes opening
    DisableCollectibleVisuals();
}

// Plays audio and particle feedback at current position
private void PlayFeedbackEffects()
{
    if (collectSound != null)
    {
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
    }

    if (collectParticlePrefab != null)
    {
        Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);
    }
}

// Hides mesh and collider so collectible disappears instantly while script continues running to open door
private void DisableCollectibleVisuals()
{
    MeshRenderer renderer = GetComponent<MeshRenderer>();
    if (renderer != null)
    {
        renderer.enabled = false;
    }

    Collider collider = GetComponent<Collider>();
    if (collider != null)
    {
        collider.enabled = false;
    }
}

// Moves target door smoothly towards target position when item is collected
private void ProcessDoorMovement()
{
    if (!isCollected || matchedDoorTransform == null)
    {
        return;
    }

    // Move target door towards target destination vector in world space
    matchedDoorTransform.position = Vector3.MoveTowards(matchedDoorTransform.position, targetDoorEndPosition, doorOpenSpeed * Time.deltaTime);

    // Once door completes movement, destroy collectible object to clean up memory
    if (Vector3.Distance(matchedDoorTransform.position, targetDoorEndPosition) < 0.01f)
    {
        Destroy(gameObject);
    }
}
}