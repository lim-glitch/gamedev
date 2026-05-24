using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHP = 3;
    public int currentHP;

    [Header("Object Type")]
    public bool isPlayer = false;
    public bool isBoss = false;

    [Header("Reward")]
    public int scoreReward = 100;
    public float progressReward = 5f;

    [Header("Invincibility")]
    public float invincibleTime = 1f;
    private float lastDamageTime = -999f;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time - lastDamageTime < invincibleTime)
            return;

        currentHP -= damage;
        lastDamageTime = Time.time;

        Debug.Log(gameObject.name + " HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    void Die()
    {
        if (isPlayer)
        {
            GameManager.Instance.GameOver();
        }
        else if (isBoss)
        {
            GameManager.Instance.AddScore(scoreReward);
            GameManager.Instance.SetFacilityProgress(100f);
            GameManager.Instance.MissionComplete();
            Destroy(gameObject);
        }
        else
        {
            GameManager.Instance.AddScore(scoreReward);
            GameManager.Instance.IncreaseFacilityProgress(progressReward);
            Destroy(gameObject);
        }
    }
}
