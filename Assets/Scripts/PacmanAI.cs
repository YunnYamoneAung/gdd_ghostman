using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class PacmanAI : MonoBehaviour
{
    public float dangerRadius = 4f;
    public float turnDecisionCooldown = 0.08f;

    [SerializeField] private AnimatedSprite deathSequence;

    private Movement movement;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    private float decideTimer = 0f;

    // Anti-loop memory
    private Vector3 lastPosition;
    private float stuckTimer;

    // --- Visited tile memory ---
    private Dictionary<Vector2Int, float> visitedTiles = new Dictionary<Vector2Int, float>();
    private float visitPenaltyDuration = 3f;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        lastPosition = transform.position;
    }

    private void Update()
    {
        decideTimer -= Time.deltaTime;

        // Track if Pac-Man is stuck in one spot
        if (Vector3.Distance(transform.position, lastPosition) < 0.2f)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f;
        }
        lastPosition = transform.position;

        // Decay visited memory
        List<Vector2Int> keys = visitedTiles.Keys.ToList();
        foreach (var k in keys)
        {
            visitedTiles[k] -= Time.deltaTime;
            if (visitedTiles[k] <= 0)
                visitedTiles.Remove(k);
        }

        if (decideTimer <= 0f)
        {
            ChooseDirection();
            decideTimer = turnDecisionCooldown;
        }

        // Rotate Pac-Man sprite to face the movement direction
        if (movement.direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
            transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        }
    }

    private void ChooseDirection()
    {
        Vector2[] dirs = new Vector2[] { Vector2.up, Vector2.left, Vector2.down, Vector2.right };
        Vector2 best = movement.direction;
        float bestScore = float.NegativeInfinity;

        Ghost[] ghosts = FindObjectsOfType<Ghost>();
        Pellet[] pellets = FindObjectsOfType<Pellet>(); // Find all pellets in maze

        foreach (var dir in dirs)
        {
            if (dir == -movement.direction) continue;
            if (movement.Occupied(dir)) continue;

            float score = 0f;

            // --- Ghost logic ---
            foreach (var g in ghosts)
            {
                if (!g.gameObject.activeInHierarchy) continue;
                float d = Vector2.Distance(transform.position, g.transform.position);

                if (g.frightened.enabled)
                {
                    // Pac-Man hunts frightened ghosts if close
                    score += Mathf.Clamp(8f - d, -2f, 8f);
                }
                else
                {
                    // Pac-Man avoids active ghosts
                    score -= Mathf.Clamp(dangerRadius - d, 0f, dangerRadius) * 2f;
                }
            }

            // --- Pellet detection (ahead of him) ---
            RaycastHit2D pelletHit = Physics2D.CircleCast(
                transform.position,
                0.25f,
                dir,
                6f,
                LayerMask.GetMask("Pellet")
            );
            if (pelletHit.collider != null) score += 4.0f;

            // --- Power Pellet detection (higher priority) ---
            RaycastHit2D powerHit = Physics2D.CircleCast(
                transform.position,
                0.25f,
                dir,
                8f,
                LayerMask.GetMask("PowerPellet")
            );
            if (powerHit.collider != null) score += 15.0f;

            // --- Global pellet goal: prefer moving toward the farthest uneaten pellet ---
            if (pellets.Length > 0)
            {
                Pellet farthest = pellets
                    .OrderByDescending(p => Vector2.Distance(transform.position, p.transform.position))
                    .FirstOrDefault();

                if (farthest != null)
                {
                    float dist = Vector2.Distance(
                        (Vector2)transform.position + dir,
                        farthest.transform.position
                    );
                    score += (1f / (dist + 1f)) * 20f; // normalize distance -> more points if heading closer to far pellet
                }
            }

            // --- Path smoothness ---
            if (dir == movement.direction) score += 0.5f;

            // --- Anti-loop penalty ---
            Vector2Int currentTile = Vector2Int.RoundToInt((Vector2)transform.position);
            if (visitedTiles.ContainsKey(currentTile))
                score -= 5f;

            if (stuckTimer > 2f && dir == movement.direction)
                score -= 3.0f;

            if (score > bestScore)
            {
                bestScore = score;
                best = dir;
            }
        }

        if (best != Vector2.zero)
        {
            movement.SetDirection(best);

            // Mark the tile as visited
            Vector2Int tile = Vector2Int.RoundToInt((Vector2)transform.position);
            visitedTiles[tile] = visitPenaltyDuration;
        }
    }

    // --- Respawn/Death handling ---

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
        movement.ResetState();
        deathSequence.enabled = false;

        movement.rb.isKinematic = false;
        visitedTiles.Clear();
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        deathSequence.enabled = true;
        deathSequence.Restart();

        movement.SetDirection(Vector2.zero, true);
        movement.rb.isKinematic = true;
        movement.rb.velocity = Vector2.zero;
        AudioManager.Instance.PlaySound(AudioManager.Instance.deathSound);
    }
}
