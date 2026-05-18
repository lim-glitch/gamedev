using UnityEngine;

public class UMLogoPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player =
            collision.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.ActivatePower(10f);

            Destroy(gameObject);
        }
    }
}