using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Era
{
    [Min(0)]
    public int age;
    [Min(0)]
    public float moveSpeed;
    [Min(0)]
    public float jumpForce;
    public Vector2 size;
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerAgeController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private List<Era> eras;
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
    [SerializeField]
    [Min(0)]
    private float minSpeedFortheStepSound = 0.1f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int points = 0;
    private PlayerAgeController playerAgeController;
    private AudioSource audioSource;
    private HashSet<Key> keys = new();
    private bool isWaitingForNextStepSound = false;
    private int stepSoundIndex = 0;
    private bool isOld;

    public event EventHandler<int> ChangedPoints;
    public event EventHandler<Key> AddedNewKey;
    public event EventHandler Caught;

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
        playerAgeController.AgeChanged += ChangedAge;

        if (jumpSound == null)
            throw new Exception($"{name}: the {nameof(jumpSound)} can't be null.");

        if (steepSounds == null || steepSounds.Length == 0)
            throw new Exception($"{name}: the {nameof(steepSounds)} can't be empty.");

        audioSource.volume *= SavingManager.Instance.GetMasterVolume(1) * SavingManager.Instance.GetSFXVolume(1);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Enemy))
        {
            if (isOld)
            {
                return;
            }

            if (collision.gameObject.TryGetComponent<EnemyController>(out var enemy))
            {
                playerAgeController.AddAge(enemy.PrisonTime);
            }

            Caught?.Invoke(this, EventArgs.Empty);
        }
    }

    private void FixedUpdate()
    {
        CheckMoveInput();
    }

    private void ChangedAge(object sender, int age)
    {
        foreach(var era in eras)
        {
            if(era.age == age)
            {
                moveSpeed = era.moveSpeed;
                jumpForce = era.jumpForce;
                transform.localScale = new Vector3 (era.size.x, era.size.y, 1f);
                return;
            }
        }
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

        if(isGrounded && !isWaitingForNextStepSound && minSpeedFortheStepSound <= Mathf.Abs(moveDirection.x))
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
}
