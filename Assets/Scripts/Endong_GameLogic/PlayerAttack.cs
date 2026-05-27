using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Sword")]
    public bool hasSword = false;
    public GameObject swordVisual;

    [Header("Shoot Light")]
    public GameObject lightProjectilePrefab;
    public Transform firePoint;
    public float shootCooldown = 0.4f;

    [Header("Audio")]
    public AudioClip swordPickupSFX;
    public AudioClip shootSFX;

    private AudioSource audioSource;

    private float lastShootTime = -999f;
    private Vector3 originalScale;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalScale = transform.localScale;

        if (swordVisual != null)
            swordVisual.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameStarted)
            return;

        if (GameManager.Instance != null && GameManager.Instance.gameEnded)
            return;

        if (!hasSword)
            return;

        if ((Input.GetKeyDown(KeyCode.P) || Input.GetMouseButtonDown(0)) &&
            Time.time - lastShootTime >= shootCooldown)
        {
            ShootLight();
        }
    }

    public void PickUpSword()
    {
        hasSword = true;

        if (swordVisual != null)
            swordVisual.SetActive(true);

        audioSource.PlayOneShot(swordPickupSFX);

        Debug.Log("Sword picked up!");
    }

    void ShootLight()
    {
        lastShootTime = Time.time;
        audioSource.PlayOneShot(shootSFX);

        if (lightProjectilePrefab == null || firePoint == null)
            return;

        GameObject light = Instantiate(lightProjectilePrefab, firePoint.position, Quaternion.identity);

        float dir = transform.localScale.x >= 0 ? 1f : -1f;

        LightProjectile projectile = light.GetComponent<LightProjectile>();
        if (projectile != null)
        {
            projectile.SetDirection(dir);
        }
    }
}