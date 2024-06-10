using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float immobileTime = 1.5f;
    [SerializeField] private float pushbackForce = 6.5f;
    [SerializeField] private float walkSoundInterval = 0.5f;
    [SerializeField] private MaterialFlash materialFlash;
    [SerializeField] private Animator animator;
    [SerializeField] private float maxYViewport = 0.96f;

    private Rigidbody rb;
    private PlayerBuild playerBuild;
    private DropCollectibles dropCollectibles;
    private Vector3 moveDirection;
    private bool canCollect = true;
    private bool canMove = true;
    private bool isInvincible = false;
    private float walkSoundTimer = 0f;
    private Camera mainCamera;

    void Start()
    { 
        rb = GetComponent<Rigidbody>();
        playerBuild = GetComponent<PlayerBuild>();
        dropCollectibles = GetComponent<DropCollectibles>();
        mainCamera = Camera.main;
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
        if(canMove && !PauseMenu.IsLevelOver)
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
        KeepPlayerInFrame();

    }

    // Prevents player from leaving the camera
    private void KeepPlayerInFrame()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.y > maxYViewport || viewportPosition.y < 0f || viewportPosition.x < 0f || viewportPosition.x > maxYViewport)
        {
            Vector3 newDirection = Vector3.zero;

            if (viewportPosition.y > maxYViewport && moveDirection.z < 0f)
            {
                newDirection.z = moveDirection.z;
            }
            else if (viewportPosition.y < 0f && moveDirection.z > 0f)
            {
                newDirection.z = moveDirection.z;
            }
            else if (viewportPosition.y > 0f && viewportPosition.y < maxYViewport)
            {
                newDirection.z = moveDirection.z;
            }

            if (viewportPosition.x > 1f && moveDirection.x < 0f)
            {
                newDirection.x = moveDirection.x;
            }
            else if (viewportPosition.x < 0f && moveDirection.x > 0f)
            {
                newDirection.x = moveDirection.x;
            }
            else if(viewportPosition.x > 0f && viewportPosition.x < 1f)
            {
                newDirection.x = moveDirection.x;
            }

            moveDirection = newDirection.normalized;
        }
    }

    // Moves player
    private void Move(Vector3 direction)
    {
        
        rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);

        if (direction != Vector3.zero)
        {
            TowerManager.instance.AttemptHideTowerBuildMenu();

            animator.SetBool("IsMoving", true);
            
            if(walkSoundTimer >= walkSoundInterval)
            {
                SoundEffectsManager.instance.PlayRandomSoundEffectClip(SoundEffectsManager.instance.playerWalk, transform, 0.05f);
                walkSoundTimer = 0f;
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        walkSoundTimer += Time.deltaTime;

    }


    public void CollideWithEnemy(Vector3 pushbackDirection)
    {
        if(isInvincible)
        {
            return;
        }

        materialFlash.StartFlashing(immobileTime);
        StartCoroutine(ImmobilizePlayer(immobileTime));

        int coinWorth = LevelManager.instance.GetCoinWorth();

        int coinsAmount = LevelManager.instance.GetCoinsAmount();
        int coinsToDrop = (int)(coinsAmount * Random.Range(0.4f, 0.6f));
        coinsToDrop -= coinsToDrop % coinWorth;

        StartCoroutine(WaitAMoment(0.1f));

        LevelManager.instance.ChangeCoinsByAmount(-coinsToDrop);
        dropCollectibles.DropAmountOfCollectibles(LevelManager.instance.coin, coinsToDrop / coinWorth);

        rb.AddForce(pushbackDirection.normalized * pushbackForce, ForceMode.Impulse);

        animator.SetBool("IsStunned", true);


    }

    private IEnumerator WaitAMoment(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Take damage when colliding with enemy
        LevelManager.instance.ChangeLivesByAmount(-1);
    }

    private IEnumerator ImmobilizePlayer(float duration)
    {
        canCollect = false;
        canMove = false;
        isInvincible = true;
        playerBuild.SetCanBuild(false);

        // Wait for given duration
        yield return new WaitForSeconds(duration);

        // After waiting variables back to true
        canCollect = true;
        canMove = true;
        isInvincible = false;
        playerBuild.SetCanBuild(true);
        animator.SetBool("IsStunned", false);

    }

    public bool CanPlayerCollect()
    {
        return canCollect;
    }

}
