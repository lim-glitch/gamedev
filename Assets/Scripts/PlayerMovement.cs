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
    private float moveInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool canWallJump;
    private bool wasGrounded;

    [Header("Power Up")]
    public bool poweredUp;
    private Coroutine powerRoutine;

    [Header("Gas")]
    public bool gasDispersing;
    public float gasCooldown = 10f;
    public bool gasOnCooldown;
    public float gasCooldownTimer;

    [Header("Liquid")]
    public bool surfaceMovementEnabled;
    public float dashCharge;
    public float maxDashCharge = 3f;

    [Header("Solid")]
    public bool tankMode;
    public float damageReduction = 0.5f;
    public int damageMultiplier = 2;
    private float moveSpeed;
    private float jumpForce;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        mainCamera = Camera.main;

        SetupAvatarStats();

        rb.freezeRotation = true;
    }

    void SetupAvatarStats()
    {
        switch (avatarType)
        {
            case AvatarType.Gas:

                moveSpeed = 5f;
                jumpForce = 8f;

                rb.gravityScale = 0.5f;
                rb.linearDamping = 2f;

                break;

            case AvatarType.Liquid:

                moveSpeed = 7f;
                jumpForce = 10f;

                rb.gravityScale = 1f;
                rb.linearDamping = 0.5f;

                break;

            case AvatarType.Solid:

                moveSpeed = 4f;
                jumpForce = 8f;

                rb.gravityScale = 1f;
                rb.linearDamping = 0f;

                break;
        }
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            moveInput * moveSpeed,
            rb.linearVelocity.y
        );

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
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
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

                rb.linearVelocity = new Vector2(
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

        powerRoutine =
            StartCoroutine(PowerRoutine(duration));
    }

    IEnumerator PowerRoutine(float duration)
    {
        poweredUp = true;

        yield return new WaitForSeconds(duration);

        poweredUp = false;
    }

    void HandleSpecialAbilities()
    {
        if (avatarType == AvatarType.Gas)
        {
            if (rb.linearVelocity.y < -2f)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    -2f
                );
            }

            if (poweredUp && Input.GetKeyDown(KeyCode.Q) && !gasOnCooldown)
            {
                StartCoroutine(GasDisperse());
            }
        }

        if (avatarType == AvatarType.Liquid)
        {
            // Existing wall slide
            if (
                isTouchingWall &&
                !isGrounded &&
                rb.linearVelocity.y < 0
            )
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    -1.5f
                );
            }

            // Powered surface movement
            if (poweredUp)
            {
                rb.gravityScale = 0.2f;
            }
            else
            {
                rb.gravityScale = 1f;
            }

            // Charged dash
            if (poweredUp)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    dashCharge += Time.deltaTime;

                    dashCharge =
                        Mathf.Clamp(
                            dashCharge,
                            0,
                            maxDashCharge
                        );
                }

                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    float dashForce =
                        dashCharge * 12f;

                    rb.linearVelocity =
                        new Vector2(
                            transform.localScale.x * dashForce,
                            rb.linearVelocity.y
                        );

                    dashCharge = 0;
                }
            }
        }

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

        IEnumerator GasDisperse()
        {
            gasOnCooldown = true;
            gasDispersing = true;

            Collider2D col = GetComponent<Collider2D>();
            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            col.enabled = false;

            Color c = sr.color;
            c.a = 0.4f;
            sr.color = c;

            yield return new WaitForSeconds(3f);

            col.enabled = true;

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
                new Vector3(1, 2, 1);
        }

        else if (moveInput < 0)
        {
            transform.localScale =
                new Vector3(-1, 2, 1);
        }
    }

    void UpdateState()
    {
        if (!isGrounded)
        {
            if (rb.linearVelocity.y > 0)
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