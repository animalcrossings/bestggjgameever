using UnityEngine;

public class MoveableBlockController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed;
    private Vector2 targetPos;
    private bool isMoving = false;
    public LayerMask whatStopsMovement;

    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // Check if movement is finished
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            // Snap to final grid
            transform.position = targetPos;
            isMoving = false;
            OnPushCallback(targetPos);
        }
    }


    protected virtual void OnPushCallback(Vector2 targetPosition)
    {
        // Can be overridden by subclasses
    }

    public bool Push(Vector2 direction)
    {
        if (isMoving) return false;

        Vector2 newTarget = targetPos + direction;

        if (!IsTileBlocked(newTarget))
        {
            targetPos = newTarget;
            isMoving = true;
            return true; 
        }
        return false; 
    }

    bool IsTileBlocked(Vector2 pos)
    {
        return Physics2D.OverlapCircle(pos, 0.2f, whatStopsMovement);
    }

    // Called by Portal to force block to stop at position
    public void ForceStopAt(Vector3 position)
    {
        targetPos = position;
        transform.position = position;
        isMoving = false;
    }

    // Check if block can be teleported (optional - for special blocks)
    public bool CanBeTeleported()
    {
        
        return true;
    }

}
