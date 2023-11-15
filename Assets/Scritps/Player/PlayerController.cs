using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
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

    [Header("Sounds")]
    [SerializeField]
    private AudioClip jumpSound;
    [SerializeField]
    private AudioClip[] steepSounds;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int points = 0;
    private PlayerAgeController playerAgeController;
    private AudioSource audioSource;
    private HashSet<Key> keys = new();
    private bool isWaitingForNextStepSound = false;
    private int stepSoundIndex = 0;

    public event EventHandler<int> ChangedPoints;
    public event EventHandler<Key> AddedNewKey;

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
        audioSource = GetComponent<AudioSource>();
        playerAgeController = GetComponent<PlayerAgeController>();
        playerAgeController.AgeTimerEnded += (s, e) => Debug.Log("Age Timer Ended");
        playerAgeController.AgeTimerStarted += (s, e) => Debug.Log("Age Timer Started");
        playerAgeController.MaxAgeLimitReached += (s, e) => Debug.Log("Max Age Reached");
        playerAgeController.RaiseModelChangeAtAge += (s, e) => Debug.Log($"Model Changed at: {e}");

        if (jumpSound == null)
            throw new Exception($"{name}: the {nameof(jumpSound)} can't be null.");

        if (steepSounds == null || steepSounds.Length == 0)
            throw new Exception($"{name}: the {nameof(steepSounds)} can't be empty.");
    }

    private void Start()
    {
        playerAgeController.StartTimer();
    }

    private void Update()
    {
        CheckGrounded();

        CheckJumpInput();
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
        CheckMoveInput();
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private void CheckJumpInput()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            audioSource.clip = jumpSound;
            audioSource.Play();
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void CheckMoveInput()
    {
        // Horizontal movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, 0);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);

        if(isGrounded && !isWaitingForNextStepSound)
        {
            audioSource.clip = steepSounds[stepSoundIndex];
            audioSource.Play();
            StartCoroutine(WaitingForNextStepSound(audioSource.clip.length));

            ++stepSoundIndex;
            if (stepSoundIndex >= steepSounds.Length)
                stepSoundIndex = 0;
        }
    }

    private IEnumerator WaitingForNextStepSound(float waitingTime)
    {
        isWaitingForNextStepSound = true;
        yield return new WaitForSeconds(waitingTime);
        isWaitingForNextStepSound = false;
    }


    public void AddKey(Key key)
    {
        if (!keys.Contains(key))
        {
            keys.Add(key);
            AddedNewKey?.Invoke(this, key);
        }
    }

    public bool HasKey(Key key)
    {
        return keys.Contains(key);
    }

    private void PlayerDeath()
    {
        transform.position = Vector3.zero;
    }
}
