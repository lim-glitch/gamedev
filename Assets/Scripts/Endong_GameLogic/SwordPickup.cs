using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.PickUpSword();
        }

        Destroy(gameObject);
    }
}
