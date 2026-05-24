using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public int damage = 1;
    public bool destroySelfAfterHit = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        DamagePlayer(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        DamagePlayer(collision.gameObject);
    }

    void DamagePlayer(GameObject other)
    {
        if (!other.CompareTag("Player")) return;

        Health playerHealth = other.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        if (destroySelfAfterHit)
        {
            Destroy(gameObject);
        }
    }
}