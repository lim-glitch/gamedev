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
        }

        if (
            avatarType == AvatarType.Liquid &&
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

        if (
            avatarType == AvatarType.Solid &&
            isGrounded &&
            !wasGrounded
        )
        {
            StartCoroutine(ShakeCamera());
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