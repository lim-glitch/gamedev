using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health enemy = collision.GetComponent<Health>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (
            collision.CompareTag("Ground") ||
            collision.CompareTag("Wall")
        )
        {
            Destroy(gameObject);
        }
    }
}