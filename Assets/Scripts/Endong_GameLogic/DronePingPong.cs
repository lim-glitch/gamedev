using UnityEngine;

public class DronePingPong : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public float speed = 2f;

    private bool movingRight = true;

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameStarted)
            return;

        if (GameManager.Instance != null && GameManager.Instance.gameEnded)
            return;

        if (leftPoint == null || rightPoint == null)
            return;

        Transform target = movingRight ? rightPoint : leftPoint;

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            movingRight = !movingRight;
        }
    }
}