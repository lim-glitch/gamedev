using UnityEngine;
using System.Collections;

//all avatar can punch, but only solid can slam
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
    public AudioClip slamSFX;
    public AudioClip punchSFX;

    private Rigidbody2D rb;
    private PlayerMovement movement;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (movement == null)
            return;

        /*if (movement.avatarType != PlayerMovement.AvatarType.Solid)
            return;*/

        if (Input.GetKeyDown(KeyCode.F))
        {
            audioSource.PlayOneShot(punchSFX);
            Punch();
        }

        if (
           movement.avatarType == PlayerMovement.AvatarType.Solid &&
           movement.poweredUp &&
           Input.GetKeyDown(KeyCode.LeftControl)
 )
        {
            GroundSlam();
        }
    }

    void Punch()
    {
        int finalDamage = baseDamage;

        if (movement.avatarType == PlayerMovement.AvatarType.Solid)
        {
            finalDamage = 3;
        }
        else if (movement.avatarType == PlayerMovement.AvatarType.Liquid)
        {
            finalDamage = 1;
        }
        else if (movement.avatarType == PlayerMovement.AvatarType.Gas)
        {
            finalDamage = 1;
        }

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
            Health enemyHealth = enemy.GetComponent<Health>();

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
                breakableWall.BreakWall(movement);
            }
        }

        Debug.Log("Solid punched with damage: " + finalDamage);
    }

    void GroundSlam()
    {
        GetComponent<Animator>().SetTrigger("Slam");

        StartCoroutine(SlamRoutine());
        audioSource.PlayOneShot(slamSFX);
    }

    IEnumerator SlamRoutine()
    {
        rb.velocity = new Vector2(0f, slamForce);

        yield return new WaitForSeconds(0.3f);

        Collider2D[] enemiesHit =
            Physics2D.OverlapCircleAll(
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
            Health enemyHealth =
                enemy.GetComponent<Health>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(finalDamage);
            }
        }

        Debug.Log("GROUND SLAM HIT!");
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