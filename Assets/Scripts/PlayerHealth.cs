using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    private float currentHealth;

    private bool isInvincible;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector3 respawnPoint;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        respawnPoint = transform.position;
    }

    void Update()
    {
        if (transform.position.y < -5f && !isInvincible)
        {
            TakeDamage(1f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        PlayerMovement movement = GetComponent<PlayerMovement>();

        if (
            movement != null &&
            movement.avatarType == PlayerMovement.AvatarType.Solid &&
            movement.tankMode
        )
        {
            damage *= movement.damageReduction;
        }

        currentHealth -= damage;

        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth > 0)
        {
            StartCoroutine(Invincibility());
            Respawn();
        }
        else
        {
            GameOver();
        }
    }

    public void TakeDamage()
    {
        TakeDamage(1f);
    }

    void Respawn()
    {
        transform.position = respawnPoint;
        rb.linearVelocity = Vector2.zero;
    }

    void GameOver()
    {
        Debug.Log("Game Over");
        gameObject.SetActive(false);
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;

        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.1f);

            sr.enabled = true;
            yield return new WaitForSeconds(0.1f);

            timer += 0.2f;
        }

        isInvincible = false;
    }
}