using UnityEngine;

public class LightProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 1;

    private float direction = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    public void SetDirection(float dir)
    {
        direction = Mathf.Sign(dir);

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        // Hit normal enemies / small robots
        Health health = other.GetComponent<Health>();
        if (health != null && !health.isPlayer && !health.isBoss)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Hit boss weak point
        BossWeakPoint weakPoint = other.GetComponent<BossWeakPoint>();
        if (weakPoint != null)
        {
            weakPoint.TakeLightDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Destroy toxic magic
        if (other.CompareTag("ToxicMagic"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }

        // Hit wall / ground
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}