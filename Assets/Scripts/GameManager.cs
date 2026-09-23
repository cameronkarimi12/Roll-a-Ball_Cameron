using TMPro;
using UnityEngine;

/************************************************************
 * COMPONENT OF: Game Manager
 * REQUIRED DEPENDENCIES: TextMeshProUGUI
 * DESCRIPTION: Handles collectible tracking and win condition
 *              for the game
 * AUTHOR: Your name
 * VERSION: 1.0
 *************************************************************/
public class GameManager : MonoBehaviour
{
    // UI text that shows collectibles remaining (assign in inspector)
    [SerializeField] private TextMeshProUGUI RemainingTextUI;

    // Victory sound (assign in inspector)
    [SerializeField] private AudioClip winClip;

    // Audio source for victory sound
    private AudioSource audioSource;

    // Tracks how many collectibles are left to collect
    private int numberOfCollectibles;

    // Prevents the win message from repeating
    private bool hasWon;

    // Initialize everything at start
    private void Start()
    {
        // Assigns the AudioSource component to the audioSource field
        audioSource = GetComponent<AudioSource>();

        // Determines how many Collectibles are in this scene
        numberOfCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;

        // Initialize the UI display
        UpdateRemainingUI();
    }

    // Called by the CollectibleController when a collectible is picked up - decrements count
    public void UpdateRemaining()
    {
        if (hasWon)
        {
            return;
        }

        if (numberOfCollectibles > 0)
        {
            numberOfCollectibles--;
        }

        UpdateRemainingUI();
    }

    private void UpdateRemainingUI()
    {
        if (RemainingTextUI == null)
        {
            return;
        }

        if (numberOfCollectibles > 0)
        {
            RemainingTextUI.text = "Collectibles Remaining: " + numberOfCollectibles;
            return;
        }

        if (hasWon)
        {
            return;
        }

        hasWon = true;
        RemainingTextUI.text = "You Win";

        if (audioSource != null && winClip != null)
        {
            audioSource.clip = winClip;
            audioSource.Play();
        }
    }
}
