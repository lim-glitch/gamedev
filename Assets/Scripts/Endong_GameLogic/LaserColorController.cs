using System.Collections;
using UnityEngine;

public class LaserColorController : MonoBehaviour
{
    [Header("Laser Object")]
    public GameObject laserLight;

    [Header("Timing")]
    public float minOffTime = 1f;
    public float maxOffTime = 3f;
    public float warningTime = 0.8f;
    public float laserOnTime = 1.2f;

    [Header("Colors")]
    public Color warningColor = new Color(1f, 0.5f, 0f, 0.6f); // orange
    public Color dangerColor = Color.red;

    private SpriteRenderer laserRenderer;
    private Collider2D laserCollider;

    void Start()
    {
        if (laserLight != null)
        {
            laserRenderer = laserLight.GetComponent<SpriteRenderer>();
            laserCollider = laserLight.GetComponent<Collider2D>();
        }

        StartCoroutine(LaserRoutine());
    }

    IEnumerator LaserRoutine()
    {
        while (true)
        {
            // OFF
            SetLaser(false, warningColor, false);

            float waitTime = Random.Range(minOffTime, maxOffTime);
            yield return new WaitForSeconds(waitTime);

            // WARNING: orange light, no damage
            SetLaser(true, warningColor, false);
            yield return new WaitForSeconds(warningTime);

            // DANGER: red light, can damage
            SetLaser(true, dangerColor, true);
            yield return new WaitForSeconds(laserOnTime);
        }
    }

    void SetLaser(bool visible, Color color, bool canDamage)
    {
        if (laserRenderer != null)
        {
            laserRenderer.enabled = visible;
            laserRenderer.color = color;
        }

        if (laserCollider != null)
        {
            laserCollider.enabled = canDamage;
        }
    }
}