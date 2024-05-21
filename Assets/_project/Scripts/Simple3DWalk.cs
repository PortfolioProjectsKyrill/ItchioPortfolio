using UnityEngine;

public class Simple3DWalk : MonoBehaviour
{
    public float baseSpeed = 5f; // Adjust the base speed as needed

    private Rigidbody _rb;
    private Vector3 movement;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Get input from the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;
    }

    private void FixedUpdate()
    {
        // Move the player
        MovePlayer(movement);
    }

    private void MovePlayer(Vector3 direction)
    {
        // Move the player based on the input and speed
        Vector3 newPosition = transform.position + direction * baseSpeed * Time.fixedDeltaTime;
        // Use Rigidbody.MovePosition for smooth interpolation
        _rb.MovePosition(newPosition);
    }
}