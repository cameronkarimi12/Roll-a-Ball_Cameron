/************************************************************
* COMPONENT OF: Collectible Prefabs
* REQUIRED DEPENDENCIES: GameManager, Collider Trigger, and Particle System Components
* DESCRIPTION: Handles player collection feedback and continuously rotates the collectible around its Y-axis.
* Author: CKarimi
* VERSION: 1.1
* VERSION 1.1: Add a behavior so that the Collectible rotates slowly about the y-axis.
*************************************************************/
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private GameObject collectParticlePrefab;

    [Tooltip("Speed at which the object rotates around the Y-axis in degrees per second.")]
    [SerializeField] private float rotationSpeed = 90f; // Speed of continuous rotation around the Y-axis

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Finds the Game Manager in the Scene
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Keeps the Update method clean by delegating rotation logic to a dedicated helper function
        RotateCollectible();
    }

    // Rotates the object continuously around its local Y-axis
    private void RotateCollectible()
    {
        // Rotates the transform around the Y-axis (Vector3.up) smoothed across frames using Time.deltaTime
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    // Call when this object collides with a trigger
    private void OnTriggerEnter(Collider other)
    {
        // Only executes if the collision was with the Player
        if (other.CompareTag("Player"))
        {
            gameManager.UpdateRemaining();

            // Spawn audio at the collectible's position (auto-destroys)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

            // Spawn particles (auto-destroys if Stop Action is set to Destroy)
            Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);

            // Safely destroy the collectible immediately
            Destroy(gameObject);
        }
    }
}