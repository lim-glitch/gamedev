using UnityEngine;

public class UMLogoPickup : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip powerPickupSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Touched: " + collision.name);

        PlayerMovement player =
            collision.GetComponent<PlayerMovement>();

        if (player != null)
        {
            Debug.Log("POWER ACTIVATED");

            player.ActivatePower(10f);

            AudioSource.PlayClipAtPoint(
    powerPickupSFX,
    transform.position
);

            Destroy(gameObject);
        }
    }
}