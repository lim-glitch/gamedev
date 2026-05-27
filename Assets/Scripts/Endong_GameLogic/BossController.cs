using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public bool bossActivated = false;

    [Header("Boss Health")]
    public int maxHP = 10;
    public int currentHP;

    [Header("Chase Settings")]
    public Transform player;
    public float moveSpeed = 0.8f;
    public float stopDistance = 2f;

    [Header("Bullet Attack")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 4f;
    private float nextShootTime;

    [Header("Toxic Magic Attack")]
    public GameObject toxicMagicPrefab;
    public Transform toxicSpawnPoint;
    public float toxicInterval = 7f;
    private float nextToxicTime;

    [Header("Spawn Small Enemies")]
    public GameObject smallEnemyPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 8f;
    private float nextSpawnTime;

    [Header("Death")]
    public GameObject deathEffect;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }
    }

    void Update()
    {
        if (!bossActivated)
            return;

        if (GameManager.Instance != null && !GameManager.Instance.gameStarted)
            return;

        if (GameManager.Instance != null && GameManager.Instance.gameEnded)
            return;

        if (player == null) return;

        ChasePlayer();
        ShootBullet();
        CastToxicMagic();
        SpawnSmallEnemy();
    }

    void ChasePlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }

        // Face player direction
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    void ShootBullet()
    {
        if (bulletPrefab == null || firePoint == null) return;

        if (Time.time >= nextShootTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();

            if (enemyBullet != null)
            {
                Vector2 direction = player.position.x > transform.position.x ? Vector2.right : Vector2.left;
                enemyBullet.SetDirection(direction);
            }

            nextShootTime = Time.time + shootInterval;
        }
    }

    void CastToxicMagic()
    {
        if (toxicMagicPrefab == null || toxicSpawnPoint == null) return;

        if (Time.time >= nextToxicTime)
        {
            Instantiate(toxicMagicPrefab, toxicSpawnPoint.position, Quaternion.identity);

            nextToxicTime = Time.time + toxicInterval;
        }
    }

    void SpawnSmallEnemy()
    {
        if (smallEnemyPrefab == null || spawnPoint == null) return;

        if (Time.time >= nextSpawnTime)
        {
            Instantiate(smallEnemyPrefab, spawnPoint.position, Quaternion.identity);

            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    public void TakeBossDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log("Boss HP: " + currentHP);

        StartCoroutine(HitFlash());

        if (currentHP <= 0)
        {
            BossDie();
        }
    }

    IEnumerator HitFlash()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(0.15f);

            spriteRenderer.color = originalColor;
        }
    }

    void BossDie()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Debug.Log("Boss defeated!");

        GameManager.Instance.MissionComplete();

        Destroy(gameObject);
    }
}