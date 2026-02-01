using UnityEngine;

public enum PortalState
{
    Active,
    Inactive,
    Occupied,
    Cooldown
}

public class Portal : MonoBehaviour
{
    [Header("Portal Set Up")]
    public PortalData portalData;
    public Portal linkedPortal;

    [Header("Exit Position Markers")]
    [Tooltip("Marker indicating the exit position for player using this portal.")]
    public Transform playerExitMarker;
    public Transform blockExitMarker;
    

    //Extension for if the portal needs to be disabled
    [Header("References")]
    public SpriteRenderer spriteRenderer;


    [Header("Settings")]
    public bool allowBlocks = true;
   
    //tweak this time to provide player with enough time to react
    public float teleportCooldown = 1.0f;
    private float lastTeleportTime = -Mathf.Infinity;
    private PortalState currentState = PortalState.Active;


    private void Awake()
    {
        if (spriteRenderer == null || portalData == null)
        {
            Debug.LogError("Portal: Missing references for initialization.");
            return;
        }

        if (playerExitMarker == null )
        {
            Debug.LogError("Portal: Exit markers are not assigned.");
            return;
        }

        if (allowBlocks && blockExitMarker == null)
        {
            Debug.LogError("Portal: Block exit marker is not assigned while blocks are allowed.");
            return;
        }
        
        spriteRenderer.sprite = portalData.portalSprite;
    }

    public bool Teleport(Transform entity)
    {
        if (currentState != PortalState.Active)
        {
            Debug.Log("Portal: Portal is inactive, teleportation aborted.");
            return false;
        }

        //checking cooldown time
        if (Time.time - lastTeleportTime < teleportCooldown)
        {
            Debug.Log("Portal: Teleportation on cooldown, teleportation aborted.");
            return false;
        }

        if (linkedPortal == null)
        {
            Debug.LogError("Portal: Linked portal is not set, teleportation aborted.");
            return false;
        }

        // Determine exit marker based on entity type
        bool isPlayer = entity.CompareTag("Player");
        bool isBlock = entity.CompareTag("Moveable");
        Vector3 exitPosition;

        if (isPlayer)
        {
            if (linkedPortal.playerExitMarker == null)
            {
                Debug.LogError($"Portal '{linkedPortal.name}': No player exit marker set!");
                return false;
            }
            exitPosition = linkedPortal.playerExitMarker.position;
        }
        else if (isBlock && allowBlocks)
        {
            if (!linkedPortal.allowBlocks)
            {
                Debug.Log($"Portal '{name}': Blocks not allowed through destination portal.");
                return false;
            }
            
            // Use block exit marker from DESTINATION portal
            if (linkedPortal.blockExitMarker == null)
            {
                Debug.LogError($"Portal '{linkedPortal.name}': No block exit marker set!");
                return false;
            }
            exitPosition = linkedPortal.blockExitMarker.position;
            
            // Check if block can be teleported
            if (entity.TryGetComponent<MoveableBlockController>(out var block))
            {
                if (!block.CanBeTeleported())
                {
                    Debug.Log($"Portal '{name}': Block cannot be teleported.");
                    return false;
                }
            }
        }
        else
        {
             Debug.LogWarning($"Portal '{name}': non teleportable entity attempted teleportation.");
            return false;
        }

        if (IsPositionBlocked(exitPosition, entity))
        {
            Debug.LogWarning($"Portal '{name}': Exit position blocked at destination!");
            return false;
        }

        entity.position = exitPosition;

        if (isPlayer && entity.TryGetComponent<PlayerController>(out var player))
        {
            player.TargetPosition.position = exitPosition;
            Debug.Log($"Portal: Teleported PLAYER from '{name}' to '{linkedPortal.name}'");
        }
        else if (isBlock && entity.TryGetComponent<MoveableBlockController>(out var block))
        {
            block.ForceStopAt(exitPosition);
            Debug.Log($"Portal: Teleported BLOCK from '{name}' to '{linkedPortal.name}'");
        }

        lastTeleportTime = Time.time;
        linkedPortal.lastTeleportTime = Time.time;

        Debug.Log("Portal: Teleportation successful.");
        return true;
    }

    private bool IsPositionBlocked(Vector3 position, Transform entityToIgnore)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.2f);
        
        foreach (Collider2D col in colliders)
        {
            // Ignore self and portals
            if (col.transform == entityToIgnore || col.CompareTag("Portal"))
                continue;
            
            // Check for walls or moveables
            if (col.CompareTag("Moveable") || col.gameObject.layer == LayerMask.NameToLayer("wall"))
            {
                return true;
            }
        }
        
        return false;
    }


    public void SetPortalState(PortalState newState)
    {
        currentState = newState;
      
    }

    public void SetActive(bool isActive)
    {
        currentState = isActive ? PortalState.Active : PortalState.Inactive;
    }

    private void OnDrawGizmos()
    {
        if (playerExitMarker != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(playerExitMarker.position, 0.3f);
            Gizmos.DrawLine(transform.position, playerExitMarker.position);
        }
        
        if (blockExitMarker != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(blockExitMarker.position, Vector3.one * 0.5f);
            Gizmos.DrawLine(transform.position, blockExitMarker.position);
        }
    }
}

