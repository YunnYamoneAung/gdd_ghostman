using UnityEngine;

public class Passage : MonoBehaviour
{
    public Transform connection; // The opposite passage exit

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name + " entered passage " + gameObject.name);

        // Save current velocity & direction (if Movement script exists)
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Movement movement = other.GetComponent<Movement>();

        Vector3 position = other.transform.position;
        position.x = connection.position.x;
        position.y = connection.position.y;

        // Teleport object to the opposite tunnel
        other.transform.position = position;

        // Restore momentum so ghost keeps moving smoothly
        if (rb != null && movement != null)
        {
            rb.MovePosition(position); // force rigidbody update
            rb.velocity = movement.direction * movement.speed * movement.speedMultiplier;
        }
    }
}
