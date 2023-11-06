using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float jumpForce = 7f;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask coinLayer;
    [SerializeField]
    private LayerMask enemyLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int collectedCoins = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        // Check for collecting coins
        Collider2D[] hitCoins = Physics2D.OverlapCircleAll(transform.position, 0.1f, coinLayer);
        foreach (Collider2D coin in hitCoins)
        {
            CollectCoin(coin.gameObject);
        }

        // Check for enemy collision
        Collider2D hitEnemy = Physics2D.OverlapCircle(transform.position, 0.1f, enemyLayer);
        if (hitEnemy != null)
        {
            PlayerDeath();
        }
    }

    private void FixedUpdate()
    {
        // Horizontal movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, 0);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }

    void CollectCoin(GameObject coin)
    {
        // Perform coin collection logic here
        collectedCoins++;
        Destroy(coin);
    }

    void PlayerDeath()
    {
        transform.position = Vector3.zero;
    }
}
