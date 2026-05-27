using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public BossController boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        boss.bossActivated = true;

        Debug.Log("Boss Activated!");

        Destroy(gameObject);
    }
}