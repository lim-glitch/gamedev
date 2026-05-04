using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    [Header("Follow Settings")]
    public float smoothSpeed = 5f;
    public float xOffset = 4f;
    public float yOffset = 1f;
    public float fixedYPosition = 0f;
    public float zPosition = -10f;

    [Header("Follow Up Down")]
    public bool followY = false;

    [Header("Level X Boundary")]
    public float levelStartX = -15f;
    public float levelEndX = 85f;

    [Header("Level Y Boundary")]
    public float levelBottomY = -3f;
    public float levelTopY = 6f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        float halfCameraWidth = cam.orthographicSize * cam.aspect;
        float halfCameraHeight = cam.orthographicSize;

        float minCameraX = levelStartX + halfCameraWidth;
        float maxCameraX = levelEndX - halfCameraWidth;

        float targetX = player.position.x + xOffset;
        targetX = Mathf.Clamp(targetX, minCameraX, maxCameraX);

        float targetY;

        if (followY)
        {
            float minCameraY = levelBottomY + halfCameraHeight;
            float maxCameraY = levelTopY - halfCameraHeight;

            targetY = player.position.y + yOffset;
            targetY = Mathf.Clamp(targetY, minCameraY, maxCameraY);
        }
        else
        {
            targetY = fixedYPosition;
        }

        Vector3 targetPosition = new Vector3(targetX, targetY, zPosition);

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}