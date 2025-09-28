using UnityEngine;

public class FruitPowerUp : MonoBehaviour
{
    public float duration = 5f;          // How long the boost lasts
    public float speedMultiplier = 1.5f; // Optional: for instant ghost speed boost

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player ghost collided
        if (other.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            Ghost ghost = other.GetComponent<Ghost>();

            if (ghost != null && ghost.movement != null)
            {
                // Apply the speed boost right away
                ghost.movement.speedMultiplier = speedMultiplier;

                // Tell GameManager to start the timer + bar
                GameManager.Instance.StartPowerup(duration);

                // Hide fruit after pickup
                gameObject.SetActive(false);

                // Optional: play a sound effect
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySound(AudioManager.Instance.eatGhostSound);
                }
            }
        }
    }
}
