using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    private bool eaten;

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        eaten = false;

        // Reduce speed when frightened
        GetComponent<Movement>().speedMultiplier = 0.6f;

        // Start flashing in second half of frightened duration
        InvokeRepeating(nameof(Flash), duration * 0.5f, 0.2f);
    }

    public override void Disable()
    {
        base.Disable();
        CancelInvoke(nameof(Flash));

        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;

        // Restore normal speed
        GetComponent<Movement>().speedMultiplier = 1f;

        eaten = false;
    }

    private void Flash()
    {
        if (blue.enabled)
        {
            blue.enabled = false;
            white.enabled = true;
        }
        else
        {
            blue.enabled = true;
            white.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled || eaten) return;

        PacmanAI pacman = other.GetComponent<PacmanAI>();
        if (pacman != null)
        {
            // Pac-Man eats the ghost
            eaten = true;

            FindObjectOfType<GameManager>().GhostEaten(GetComponent<Ghost>());
        }
    }
}
