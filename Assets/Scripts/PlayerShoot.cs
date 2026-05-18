using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public int projectileDamage = 1;
    public float shootCooldown = 0.3f;

    private float cooldownTimer;
    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = shootCooldown;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("Projectile prefab or fire point is missing.");
            return;
        }

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Projectile projectile =
            projectileObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            int finalDamage = projectileDamage;

            if (
                movement != null &&
                movement.avatarType == PlayerMovement.AvatarType.Solid &&
                movement.tankMode
            )
            {
                finalDamage *= movement.damageMultiplier;
            }

            projectile.damage = finalDamage;
        }

        Rigidbody2D projectileRb =
            projectileObject.GetComponent<Rigidbody2D>();

        if (projectileRb != null)
        {
            float direction = transform.localScale.x > 0 ? 1f : -1f;

            projectileRb.linearVelocity =
                new Vector2(direction * projectileSpeed, 0f);
        }
    }
}