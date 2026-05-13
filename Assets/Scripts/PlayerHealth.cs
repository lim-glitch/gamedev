using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
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
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

        currentHealth--;

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