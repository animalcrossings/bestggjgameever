using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public Transform TargetPosition;
    public LayerMask whatStopsMovement;
    public float teleportCooldownTime = 0.5f;
    
    [Header("Interaction Settings")]
    [SerializeField] public LayerMask interactableLayer;
    private const float INTERACT_DISTANCE = 1.5f;
    private const float TOOLTIP_DISTANCE = 5.0f;

    [Header("Input")]
    public InputActionReference moveAction;

    private float teleportTimer;
    private Vector3 lastFacingDirection = Vector3.down; // Default facing direction

    void Start()
    {
        // Detach target to move independently
        TargetPosition.parent = null;
        // Snap target to integer grid to prevent drift
        TargetPosition.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0f);
    }

    void Update()
    {
        CheckInteractionPrompts();

        // Teleport cooldown
        if (teleportTimer > 0)
        {
            teleportTimer -= Time.deltaTime;
            return;
        }

        // Smoothly move visual player to target
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition.position, moveSpeed * Time.deltaTime);

        // Only allow new input if we are very close to the target (grid lock)
        if (Vector3.Distance(transform.position, TargetPosition.position) <= .05f)
        {
            HandleMovementInput();
        }
    }

    private void HandleMovementInput()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // Prioritize X axis movement, then Y (prevents diagonal movement if that's desired)
        if (Mathf.Abs(input.x) > 0.5f)
        {
            AttemptMove(new Vector3(Mathf.Sign(input.x), 0f, 0f));
        }
        else if (Mathf.Abs(input.y) > 0.5f)
        {
            AttemptMove(new Vector3(0f, Mathf.Sign(input.y), 0f));
        }
    }

    private void AttemptMove(Vector3 direction)
    {
        // Update facing direction for interactions
        lastFacingDirection = direction;

        // Check if the target tile is blocked
        if (!Physics2D.OverlapCircle(TargetPosition.position + direction, 0.2f, whatStopsMovement))
        {
            // Tile is free, move target
            TargetPosition.position += direction;
            AudioManager.Instance.PlayFootstepSound(); // Play sound ONCE per step
        }
        else
        {
            // Tile is blocked, check for interactables (Pushable/Teleporter)
            HandleObstacleInteraction(direction);
        }
    }

    private void HandleObstacleInteraction(Vector3 direction)
    {
        // Cast ray from current position to see what we hit
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, whatStopsMovement);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Moveable"))
            {
                MoveableBlockController block = hit.collider.GetComponent<MoveableBlockController>();
                if (block != null && block.Push(direction))
                {
                    // If block moved successfully, we can move into its space
                    TargetPosition.position += direction;
                    AudioManager.Instance.PlayFootstepSound();
                }
            }
            else if (hit.collider.CompareTag("Teleporter"))
            {
                PortalController portal = hit.collider.GetComponent<PortalController>();
                if (portal != null)
                {
                    PerformTeleport(portal);
                }
            }
        }
    }

    private void PerformTeleport(PortalController portal)
    {
        transform.position = portal.playerTPblock.transform.position;
        TargetPosition.position = portal.playerTPblock.transform.position;
        portal.cameraTP();
        teleportTimer = teleportCooldownTime;
    }

    // --- Interaction Logic ---

    public GameObject GetLookingAt(float distance, LayerMask layerMask)
    {
        // FIX: Use lastFacingDirection instead of calculating from TargetPosition
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastFacingDirection, distance, layerMask);
        return hit.collider != null ? hit.collider.gameObject : null;
    }

    // (GetNearby and GetClosestInteractable left unchanged as they looked fine)
    public GameObject[] GetNearby(float distance, LayerMask layerMask)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, distance, layerMask);
        return hits.Length == 0 ? null : Array.ConvertAll(hits, hit => hit.gameObject);
    }

    public void CheckInteractionPrompts()
    {
        // Optimization: Only run this check if we aren't moving? 
        // For now, kept in Update but cleaned up.
        GameObject lookingAt = GetLookingAt(TOOLTIP_DISTANCE, interactableLayer);
        
        if (lookingAt != null)
        {
            IInteractable interactable = lookingAt.GetComponent<IInteractable>();
            interactable?.ShowTooltip(true);
        }
    }

    public void TryInteract()
    {
        GameObject lookingAt = GetLookingAt(INTERACT_DISTANCE, interactableLayer);
        if (lookingAt != null)
        {
            IInteractable interactable = lookingAt.GetComponent<IInteractable>();
            interactable?.OnInteract();
        }
    }

    // (OnTriggerEnter2D left unchanged)
    public void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.CompareTag("Item")) GameManager.Instance.HandleItemPickup(collision.gameObject);
         else if (collision.CompareTag("Door")) GameManager.Instance.TryOpenDoor(collision.gameObject);
         else if (collision.CompareTag("Mask")) GameManager.Instance.HandleItemPickup(collision.gameObject);
         else if (collision.CompareTag("Portal"))
         {
             // Added null check for safety
             Portal portal = collision.GetComponent<Portal>();
             if (portal) GameManager.Instance.HandlePortalEntry(collision.gameObject, transform);
         }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, INTERACT_DISTANCE);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, TOOLTIP_DISTANCE);
        
        // Visualize facing direction
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + lastFacingDirection);
    }
}