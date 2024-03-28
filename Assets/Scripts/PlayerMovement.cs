using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    { 
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GatherInput();

        // Rotate player to face movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }

    void FixedUpdate()
    {
        // Move the player based on input
        Move(moveDirection);
    }

    void GatherInput()
    {
        // Get the input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Calculate movement direction
        moveDirection = new Vector3(moveX, 0f, moveY).normalized;
    }

    // Moves player
    void Move(Vector3 direction)
    {
        // Apply movement to rigidbody
        rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);

    }
}
