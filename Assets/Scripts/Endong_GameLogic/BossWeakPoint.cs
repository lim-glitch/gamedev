using UnityEngine;

public class BossWeakPoint : MonoBehaviour
{
    [Header("Boss Reference")]
    public BossController boss;

    [Header("Damage Settings")]
    public int damageToBoss = 1;
    public float hitCooldown = 1f;

    private float lastHitTime = -999f;

    public void TakeLightDamage(int lightDamage)
    {
        if (Time.time - lastHitTime < hitCooldown)
            return;

        lastHitTime = Time.time;

        if (boss != null)
        {
            boss.TakeBossDamage(damageToBoss * lightDamage);
            Debug.Log(gameObject.name + " damaged boss: " + (damageToBoss * lightDamage));
        }
    }
}