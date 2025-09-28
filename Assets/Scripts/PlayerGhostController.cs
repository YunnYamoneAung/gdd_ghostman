using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerGhostController : MonoBehaviour
{
    private Movement movement;

    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        Vector2 inputDir = Vector2.zero;

        if (Input.GetKey(up) || Input.GetKey(KeyCode.W)) inputDir = Vector2.up;
        else if (Input.GetKey(down) || Input.GetKey(KeyCode.S)) inputDir = Vector2.down;
        else if (Input.GetKey(left) || Input.GetKey(KeyCode.A)) inputDir = Vector2.left;
        else if (Input.GetKey(right) || Input.GetKey(KeyCode.D)) inputDir = Vector2.right;

        if (inputDir != Vector2.zero)
            movement.SetDirection(inputDir, true);
    }
}
