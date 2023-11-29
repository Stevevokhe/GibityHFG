using System;
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
    public bool isOld;
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

    [Header("Animation")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PlayerAnimationEventController animationEventController;
    [SerializeField]
    [Range(0,1)]
    [Tooltip("For rotation and sounds. It is percentage.")]
    private float minSpeed = 0.25f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int points = 0;
    private PlayerAgeController playerAgeController;
    private AudioSource audioSource;
    private HashSet<Key> keys = new();
    private int stepSoundIndex = 0;
    private bool isOld;
    private float volume;
    private Vector3 scale;
    private Era selectedEra = null;

    public event EventHandler<int> ChangedPoints;
    public event EventHandler Died;
    public event EventHandler<Key> AddedNewKey;
    public event EventHandler<int> Caught;
    public event EventHandler GotOlder;

    private const string SpeedId = "Speed";
    private const string IsGroundedId = "IsGrounded";

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
        playerAgeController.MaxAgeLimitReached += (s, e) => Died?.Invoke(this, EventArgs.Empty);
        playerAgeController.AgeChanged += ChangedAge;

        if (jumpSound == null)
            throw new Exception($"{name}: the {nameof(jumpSound)} can't be null.");

        if (steepSounds == null || steepSounds.Length == 0)
            throw new Exception($"{name}: the {nameof(steepSounds)} can't be empty.");

        if (animator == null)
            throw new Exception($"{name}: the {nameof(animator)} can't be null.");

        if (animationEventController == null)
            throw new Exception($"{name}: the {nameof(animationEventController)} can't be null.");

        audioSource.volume *= SavingManager.Instance.GetMasterVolume(1) * SavingManager.Instance.GetSFXVolume(1);
        volume = audioSource.volume;
        scale = transform.localScale;

        animationEventController.Step += UseStepSound;
    }

    public void Start()
    {
        playerAgeController.StartTimer();
    }

    private void Update()
    {
        CheckGrounded();

        CheckJumpInput();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Enemy))
        {
            if (isOld)
            {
                return;
            }

            if (collision.TryGetComponent<EnemyController>(out var enemy))
            {
                playerAgeController.AddAge(enemy.PrisonTime);
            }

            Caught?.Invoke(this, enemy.PrisonTime);
        }
    }

    private void FixedUpdate()
    {
        CheckMoveInput();
    }

    private void LateUpdate()
    {
        FixAnimation();
    }

    private void ChangedAge(object sender, int age)
    {
        Era newEra = null;
        foreach(var era in eras)
        {
            if(era.age <= age && (newEra == null || era.age > newEra.age))
            {
                newEra = era;
            }
        }

        if(newEra != null && selectedEra != newEra)
        {
            GotOlder?.Invoke(this, EventArgs.Empty);
            selectedEra = newEra;
            moveSpeed = selectedEra.moveSpeed;
            jumpForce = selectedEra.jumpForce;
            transform.localScale = new Vector3(selectedEra.size.x, selectedEra.size.y, transform.localScale.z);
            scale = transform.localScale;
            isOld = selectedEra.isOld;
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        animator.SetBool(IsGroundedId, isGrounded);
    }

    private void CheckJumpInput()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            audioSource.clip = jumpSound;
            audioSource.volume = volume;
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
    }

    private void FixAnimation()
    {
        float move = rb.velocity.x / moveSpeed;
        float absMove = Mathf.Abs(move);
        float absJump = rb.velocity.y / moveSpeed;
        animator.SetFloat(SpeedId, absMove);

        if(absMove < minSpeed && absJump < minSpeed)
            audioSource.volume = 0;

        if (move > minSpeed)
            transform.localScale = scale;
        else if (move < -minSpeed)
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
    }

    private void UseStepSound(object sender, EventArgs e)
    {
        audioSource.clip = steepSounds[stepSoundIndex];
        audioSource.volume = volume;
        audioSource.Play();

        ++stepSoundIndex;
        if (stepSoundIndex >= steepSounds.Length)
            stepSoundIndex = 0;
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
