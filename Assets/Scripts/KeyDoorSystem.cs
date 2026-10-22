using UnityEngine;

public class KeyDoorSystem : MonoBehaviour
{
    public string keyTag = "Player";
    public GameObject door;
    public bool destroyKeyOnTouch = true;

    private void Start()
    {
        // Door starts visible
        if (door != null)
            door.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detected object: " + other.name +
            " | Tag: " + other.tag +
            " | Expected: " + keyTag);

        if (other.CompareTag(keyTag))
        {
            Debug.Log("KEY COLLECTED! Hiding door.");

            // Hide the door when the player touches the key
            if (door != null)
                door.SetActive(false);
            else
                Debug.LogError("DOOR IS NOT ASSIGNED!");

            // Remove the key after collecting it
            if (destroyKeyOnTouch)
                Destroy(gameObject);
        }
    }
}