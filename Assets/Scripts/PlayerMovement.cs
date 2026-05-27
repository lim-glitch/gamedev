using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum AvatarType
    {
        Gas,
        Liquid,
        Solid
    }

    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Fall
    }

    public AvatarType avatarType;

    public PlayerState currentState;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector3 originalScale;

    private float moveInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool canWallJump;
    private bool wasGrounded;

    private float moveSpeed;
    private float jumpForce;

    [Header("Power Up")]
    public bool poweredUp;
    private Coroutine powerRoutine;

    [Header("Gas")]
    public bool gasDispersing;
    public float gasCooldown = 10f;
    public bool gasOnCooldown;
    public float gasCooldownTimer;

    [Header("Liquid")]
    public float dashCharge;
    public float maxDashCharge = 3f;

    [Header("Solid")]
    public bool tankMode;
    public float damageReduction = 0.5f;
    public int damageMultiplier = 2;

    [Header("Audio")]
    public AudioClip punchSFX;
    public AudioClip dashSFX;
    public AudioClip slamSFX;
    public AudioClip gasSFX;

    private AudioSource audioSource;

    private bool isDashing;
    private Animator anim;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;

        mainCamera = Camera.main;

        SetupAvatarStats();

        rb.freezeRotation = true;

        anim = GetComponent<Animator>();
    }

    void SetupAvatarStats()
    {
        switch (avatarType)
        {
            case AvatarType.Gas:

                moveSpeed = 5f;
                jumpForce = 8f;

                rb.gravityScale = 0.5f;
                rb.drag = 2f;

                break;

            case AvatarType.Liquid:

                moveSpeed = 7f;
                jumpForce = 10f;

                rb.gravityScale = 1f;
                rb.drag = 0.5f;

                break;

            case AvatarType.Solid:

                moveSpeed = 4f;
                jumpForce = 8f;

                rb.gravityScale = 1f;
                rb.drag = 0f;

                break;
        }
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        anim.SetBool("IsGrounded", isGrounded);

        /*rb.velocity = new Vector2(
            moveInput * moveSpeed,
            rb.velocity.y
        );*/

        if (!isDashing)
        {
            rb.velocity = new Vector2(
                moveInput * moveSpeed,
                rb.velocity.y
            );
        }

        HandleJump();

        HandleSpecialAbilities();

        FlipCharacter();

        UpdateState();

        wasGrounded = isGrounded;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(
                    rb.velocity.x,
                    jumpForce
                );
            }

            else if (
                avatarType == AvatarType.Liquid &&
                isTouchingWall &&
                canWallJump
            )
            {
                float jumpDirection =
                    transform.localScale.x > 0 ? -8f : 8f;

                rb.velocity = new Vector2(
                    jumpDirection,
                    jumpForce
                );

                canWallJump = false;
            }
        }
    }

    public void ActivatePower(float duration)
    {
        if (powerRoutine != null)
        {
            StopCoroutine(powerRoutine);
        }

        powerRoutine = StartCoroutine(PowerRoutine(duration));
    }

    IEnumerator PowerRoutine(float duration)
    {
        poweredUp = true;

        yield return new WaitForSeconds(duration);

        poweredUp = false;
    }

    void HandleSpecialAbilities()
    {
        // GAS
        if (avatarType == AvatarType.Gas)
        {
            if (rb.velocity.y < -2f)
            {
                rb.velocity = new Vector2(
                    rb.velocity.x,
                    -2f
                );
            }

            if (poweredUp && Input.GetKeyDown(KeyCode.Q) && !gasOnCooldown)
            {
                StartCoroutine(GasDisperse());
            }
        }

        // LIQUID
        if (avatarType == AvatarType.Liquid)
        {
            if (
                isTouchingWall &&
                !isGrounded &&
                rb.velocity.y < 0
            )
            {
                rb.velocity = new Vector2(
                    rb.velocity.x,
                    -1.5f
                );
            }

            /*if (poweredUp)
            {
              rb.gravityScale = 0.2f;
             }
            else
             {
               rb.gravityScale = 1f;
             }*/

            rb.gravityScale = 1f;

            // DASH
            if (poweredUp)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
                {
                    StartCoroutine(LiquidDash());
                }
            }

        }

        // SOLID
        if (avatarType == AvatarType.Solid)
        {
            if (poweredUp)
            {
                tankMode = true;
                moveSpeed = 2.5f;
            }
            else
            {
                tankMode = false;
                moveSpeed = 4f;
            }

            if (isGrounded && !wasGrounded)
            {
                StartCoroutine(ShakeCamera());
            }
        }
    }
    IEnumerator LiquidDash()
    {
        anim.SetTrigger("Dash");
        audioSource.PlayOneShot(dashSFX);

        isDashing = true;

        rb.gravityScale = 0f;

        float direction =
            transform.localScale.x > 0 ? 1f : -1f;

        rb.velocity = new Vector2(direction * 22f, 0f);

        yield return new WaitForSeconds(0.25f);

        rb.gravityScale = 1f;

        isDashing = false;
    }
    IEnumerator GasDisperse()
    {
        gasOnCooldown = true;
        gasDispersing = true;
        anim.SetTrigger("Disperse");

        audioSource.PlayOneShot(gasSFX);

        Collider2D col = GetComponent<Collider2D>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        //col.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Ghost");

        Color c = sr.color;
        c.a = 0.4f;
        sr.color = c;

        yield return new WaitForSeconds(3f);

        gameObject.layer = LayerMask.NameToLayer("Player");

        c.a = 1f;
        sr.color = c;

        gasDispersing = false;

        gasCooldownTimer = gasCooldown;

        while (gasCooldownTimer > 0f)
        {
            gasCooldownTimer -= Time.deltaTime;
            yield return null;
        }

        gasCooldownTimer = 0f;
        gasOnCooldown = false;
    }

    IEnumerator ShakeCamera()
    {
        Vector3 originalPosition =
            mainCamera.transform.position;

        float duration = 0.08f;
        float magnitude = 0.15f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x =
                Random.Range(-1f, 1f) * magnitude;

            float y =
                Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.position =
                new Vector3(
                    originalPosition.x + x,
                    originalPosition.y + y,
                    originalPosition.z
                );

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.position =
            originalPosition;
    }

    void FlipCharacter()
    {
        if (moveInput > 0)
        {
            transform.localScale =
                new Vector3(
                    Mathf.Abs(originalScale.x),
                    originalScale.y,
                    originalScale.z
                );
        }

        else if (moveInput < 0)
        {
            transform.localScale =
                new Vector3(
                    -Mathf.Abs(originalScale.x),
                    originalScale.y,
                    originalScale.z
                );
        }
    }

    void UpdateState()
    {
        if (!isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                currentState = PlayerState.Jump;
            }
            else
            {
                currentState = PlayerState.Fall;
            }
        }
        else
        {
            if (moveInput != 0)
            {
                currentState = PlayerState.Run;
            }
            else
            {
                currentState = PlayerState.Idle;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;

                    canWallJump = true;
                }

                if (
                    avatarType == AvatarType.Liquid &&
                    Mathf.Abs(contact.normal.x) > 0.5f
                )
                {
                    isTouchingWall = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            isTouchingWall = false;
        }
    }
}