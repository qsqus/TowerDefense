using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float immobileTime = 1.5f;
    [SerializeField] private float pushbackForce = 6.5f;

    private Rigidbody rb;
    private PlayerBuild playerBuild;
    private DropCollectibles dropCollectibles;
    private Vector3 moveDirection;
    private bool canCollect = true;
    private bool canMove = true;
    private bool isInvincible = false;

    void Start()
    { 
        rb = GetComponent<Rigidbody>();
        playerBuild = GetComponent<PlayerBuild>();
        dropCollectibles = GetComponent<DropCollectibles>();
    }

    void Update()
    {
        GatherInput();

        // Rotate player to face movement direction
        if (moveDirection != Vector3.zero && canMove)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }

    void FixedUpdate()
    {
        // Move the player based on input
        if(canMove)
        {
            Move(moveDirection);
        }
    }

    // Gathers movement input
    private void GatherInput()
    {
        // Get the input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Calculate movement direction
        moveDirection = new Vector3(moveX, 0f, moveY).normalized;
    }

    // Moves player
    private void Move(Vector3 direction)
    {
        // Apply movement to rigidbody
        rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);

    }

    
    public void CollideWithEnemy(Vector3 pushbackDirection)
    {
        if(isInvincible)
        {
            return;
        }

        StartCoroutine(ImmobilizePlayer(immobileTime));

        int coinsAmount = LevelManager.instance.GetCoinsAmount();
        int coinsToDrop = (int)(coinsAmount * Random.Range(0.4f, 0.6f));

        LevelManager.instance.ChangeCoinsByAmount(-coinsToDrop);
        dropCollectibles.DropAmountOfCollectibles(LevelManager.instance.coin, coinsToDrop);

        rb.AddForce(pushbackDirection.normalized * pushbackForce, ForceMode.Impulse);
    }

    private IEnumerator ImmobilizePlayer(float duration)
    {
        // Set the boolean variable to false
        canCollect = false;
        canMove = false;
        isInvincible = true;
        playerBuild.SetCanBuild(false);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // After waiting, set the boolean variable back to true
        canCollect = true;
        canMove = true;
        isInvincible = false;
        playerBuild.SetCanBuild(true);

    }

    public bool CanPlayerCollect()
    {
        return canCollect;
    }

}
