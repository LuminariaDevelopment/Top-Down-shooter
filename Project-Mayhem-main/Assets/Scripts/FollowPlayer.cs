using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float smoothing = 5f;
    public LayerMask obstacleMask;
    public float collisionDistance = 0.2f; // Adjust this distance as needed

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = player.position + offset;

        // Check for obstacles between the camera and player
        RaycastHit hit;
        if (Physics.Linecast(player.position, targetPosition, out hit, obstacleMask))
        {
            // If an obstacle is hit, adjust the target position based on collisionDistance
            float adjustedX = hit.point.x - offset.normalized.x * collisionDistance;
            float adjustedZ = hit.point.z - offset.normalized.z * collisionDistance;
            targetPosition = new Vector3(adjustedX, targetPosition.y, adjustedZ);
        }

        // Use Lerp to smoothly interpolate between current and target position (only horizontal)
        Vector3 newPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        // Set the position only if there is no collision
        if (!Physics.Linecast(player.position, newPosition, obstacleMask))
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, smoothing * Time.deltaTime);
        }
    }
}
