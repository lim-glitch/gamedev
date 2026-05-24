using UnityEngine;

public class ProgressTrigger : MonoBehaviour
{
    public float progressValue = 25f;
    public bool destroyAfterUse = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.SetFacilityProgress(progressValue);

        if (destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}