using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public LayerMask obstacleLayer;   // Only assign "Obstacle" layer here in Inspector

    public Vector2 initialDirection;
    public Rigidbody2D rb { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        enabled = true;
    }

    private void Update()
    {
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = rb.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;
        rb.MovePosition(pos + translation);
    }

    public void SetDirection(Vector2 dir, bool forced = false)
    {
        if (forced || !Occupied(dir))
        {
            direction = dir;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = dir;
        }
    }

    public bool Occupied(Vector2 dir)
    {
        // Box size depends on current position
        Vector2 size = Vector2.one * 0.75f;
        Vector2 offset = dir * 0.5f;

        // Check only against "Obstacle" layer
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + (Vector3)offset, size, 0f, dir, 0f, obstacleLayer);

        return hit.collider != null;
    }
}
