using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAgeController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float jumpForce = 7f;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask itemLayer;
    [SerializeField]
    private LayerMask enemyLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int points = 0;
    private PlayerAgeController playerAgeController;

    public event EventHandler<int> ChangedPoints;

    public int Points
    {
        get => points;
        set
        {
            if (points == value)
                return;

            points = value;
            ChangedPoints?.Invoke(this, points);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAgeController = GetComponent<PlayerAgeController>();
        playerAgeController.AgeTimerEnded += (s, e) => Debug.Log("Age Timer Ended");
        playerAgeController.AgeTimerStarted += (s, e) => Debug.Log("Age Timer Started");
        playerAgeController.MaxAgeLimitReached += (s, e) => Debug.Log("Max Age Reached");
        playerAgeController.RaiseModelChangeAtAge += (s, e) => Debug.Log($"Model Changed at: {e}");
    }

    private void Start()
    {
        playerAgeController.StartTimer();
    }

    private void Update()
    {
        // Check for grounded state
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Jump input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            PlayerDeath();
            return;
        }
    }

    private void FixedUpdate()
    {
        // Horizontal movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, 0);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }

    private void PlayerDeath()
    {
        transform.position = Vector3.zero;
    }
}
