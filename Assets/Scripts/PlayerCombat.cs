using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Punch Settings")]
    public int baseDamage = 1;
    public float punchRange = 1.2f;
    public Transform punchPoint;
    public LayerMask enemyLayer;
    public LayerMask breakableLayer;

    [Header("Ground Slam Settings")]
    public float slamForce = -20f;
    public float slamRadius = 1.5f;
    public int slamDamage = 2;

    private Rigidbody2D rb;
    private PlayerMovement movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (movement == null)
            return;

        if (movement.avatarType != PlayerMovement.AvatarType.Solid)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Punch();
        }

        if (movement.poweredUp && Input.GetKeyDown(KeyCode.LeftControl))
        {
            GroundSlam();
        }
    }

    void Punch()
    {
        int finalDamage = baseDamage;

        if (movement.tankMode)
        {
            finalDamage *= movement.damageMultiplier;
        }

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(
            punchPoint.position,
            punchRange,
            enemyLayer
        );

        foreach (Collider2D enemy in enemiesHit)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(finalDamage);
            }
        }

        Collider2D[] wallsHit = Physics2D.OverlapCircleAll(
            punchPoint.position,
            punchRange,
            breakableLayer
        );

        foreach (Collider2D wall in wallsHit)
        {
            BreakableWall breakableWall = wall.GetComponent<BreakableWall>();

            if (breakableWall != null)
            {
                breakableWall.BreakWall();
            }
        }

        Debug.Log("Solid punched with damage: " + finalDamage);
    }

    void GroundSlam()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            slamForce
        );

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(
            transform.position,
            slamRadius,
            enemyLayer
        );

        int finalDamage = slamDamage;

        if (movement.tankMode)
        {
            finalDamage *= movement.damageMultiplier;
        }

        foreach (Collider2D enemy in enemiesHit)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(finalDamage);
            }
        }

        Debug.Log("Solid ground slam activated.");
    }

    private void OnDrawGizmosSelected()
    {
        if (punchPoint != null)
        {
            Gizmos.DrawWireSphere(punchPoint.position, punchRange);
        }

        Gizmos.DrawWireSphere(transform.position, slamRadius);
    }
}