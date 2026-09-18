using UnityEngine;
/************************************************************
* COMPONENT OF: Collectible Prefabs
* REQUIRED DEPENDENCIES: GameManager, Collider Trigger, and Particle System Components
* DESCRIPTION: This script handles player collection logic, plays feedback audio/particles
* Author: CKarimi
* VERSION: 1.0
*************************************************************/
public class CollectibleController : MonoBehaviour
{ [SerializeField] private AudioClip collectSound;
[SerializeField] private GameObject collectParticlePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }// Call with this object collides with a trigger
    private void OnTriggerEnter(Collider other)
    {
        // Only executes if the collision was with the Player
        if (other.CompareTag("Player"))
        {
            // Spawn audio at the collectible's position (auto-destroys)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

            // Spawn particles (auto-destroys if Stop Action is set to Destroy)
            Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);

            // Safely destroy the collectible immediately
            Destroy(gameObject);
        }
    }

}
