using UnityEngine;

public class MazeAI : MonoBehaviour
{
    public Transform target;             // The TreasureChest
    public float moveSpeed = 2.5f;
    public float castDistance = 0.6f;
    public float detectionRadius = 0.4f;
    public LayerMask obstacleLayer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target == null) return;

        Vector2 toTarget = ((Vector2)target.position - rb.position).normalized;
        Vector2 moveDir = toTarget;

        // If there's a wall directly in front
        if (IsObstacleAhead(toTarget))
        {
            // Try hugging the wall to slide along it
            moveDir = SlideAlongWall(toTarget);
        }

        Vector2 nextPos = rb.position + moveDir * moveSpeed * Time.deltaTime;
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = rb.Cast(moveDir, hits, moveSpeed * Time.deltaTime);

        if (hitCount == 0)
        {
            rb.MovePosition(nextPos);
        }
    }

    Vector2 SlideAlongWall(Vector2 blockedDirection)
    {
        Vector2 perpLeft = Vector2.Perpendicular(blockedDirection);
        Vector2 perpRight = -perpLeft;

        Vector2[] slideDirections = {
        (blockedDirection + perpLeft).normalized,
        (blockedDirection + perpRight).normalized,
        perpLeft.normalized,
        perpRight.normalized
    };

        foreach (var dir in slideDirections)
        {
            if (!IsObstacleAhead(dir))
                return dir;
        }

        // Fully boxed in
        return Vector2.zero;
    }

    bool IsObstacleAhead(Vector2 direction)
    {
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int count = rb.Cast(direction, hits, castDistance);

        if (count > 0)
        {
            var hitObj = hits[0].collider.gameObject;
            if (hitObj.CompareTag("Goal")) return false; // Don't treat chest as obstacle

            Debug.DrawRay(rb.position, direction * castDistance, Color.red);
            return true;
        }

        Debug.DrawRay(rb.position, direction * castDistance, Color.green);
        return false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
