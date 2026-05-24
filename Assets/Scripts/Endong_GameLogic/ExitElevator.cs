using UnityEngine;

public class ExitElevator : MonoBehaviour
{
    public bool requireMissionStarted = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (requireMissionStarted)
        {
            if (GameManager.Instance != null && !GameManager.Instance.gameStarted)
                return;
        }

        GameManager.Instance.MissionComplete();
    }
}