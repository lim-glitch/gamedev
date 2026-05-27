using UnityEngine;

public class GuardPatrolShooter : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform leftPoint;
    public Transform rightPoint;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 3f;
    public float detectionRange = 8f;
    public float lineOfSightHeight = 1.5f;

    private Transform targetPoint;
    private int moveDirection = 1; // 1 = right, -1 = left
    private float nextShootTime = 0f;
    private Vector3 originalScale;

    void Start()
    {
        targetPoint = rightPoint;
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameStarted)
            return;

        if (GameManager.Instance != null && GameManager.Instance.gameEnded)
            return;

        Patrol();
        ShootIfPlayerInSight();
    }

    void Patrol()
    {
        if (leftPoint == null || rightPoint == null)
            return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (targetPoint == rightPoint)
            {
                targetPoint = leftPoint;
                moveDirection = -1;
            }
            else
            {
                targetPoint = rightPoint;
                moveDirection = 1;
            }

            FlipGuard();
        }
    }

    void FlipGuard()
    {
        transform.localScale = new Vector3(
            Mathf.Abs(originalScale.x) * moveDirection,
            originalScale.y,
            originalScale.z
        );

        if (firePoint != null)
        {
            firePoint.localPosition = new Vector3(
                Mathf.Abs(firePoint.localPosition.x) * moveDirection,
                firePoint.localPosition.y,
                firePoint.localPosition.z
            );
        }
    }

    void ShootIfPlayerInSight()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null || bulletPrefab == null || firePoint == null)
            return;

        float xDistance = Mathf.Abs(player.transform.position.x - transform.position.x);
        float yDistance = Mathf.Abs(player.transform.position.y - transform.position.y);

        if (xDistance <= detectionRange && yDistance <= lineOfSightHeight)
        {
            if (Time.time >= nextShootTime)
            {
                Shoot();
                nextShootTime = Time.time + shootInterval;
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();

        if (enemyBullet != null)
        {
            enemyBullet.SetDirection(new Vector2(moveDirection, 0));
        }

        Debug.Log("Guard shoots " + (moveDirection == 1 ? "right" : "left"));
    }
}