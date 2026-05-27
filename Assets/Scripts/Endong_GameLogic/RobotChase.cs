using UnityEngine;

public class RobotChase : MonoBehaviour
{
    public float speed = 2.5f;
    public float chaseRange = 8f;
    public float stopDistance = 0.8f;

    private Transform player;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameStarted)
            return;

        if (GameManager.Instance != null && GameManager.Instance.gameEnded)
            return;

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseRange && distance > stopDistance)
        {
            Vector2 targetPosition = new Vector2(
                player.position.x,
                transform.position.y
            );

            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
        }
    }
}