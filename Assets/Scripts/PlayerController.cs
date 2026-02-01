using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveAction;
    public float moveSpeed;
    public Transform TargetPosition;
    public LayerMask whatStopsMovement;
    [SerializeField] public LayerMask interactableLayer;

    private const float INTERACT_DISTANCE = 1.5f;
    private const float TOOLTIP_DISTANCE = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TargetPosition.parent = null;
    }

    void PlayFootStepSound()
    {
        AudioManager.Instance.PlayFootstepSound();
    }

    

    // Update is called once per frame
    void Update()
    {

        CheckInteractionPrompts();
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition.position, moveSpeed * Time.deltaTime);
        //print(TargetPosition.position);
        //Debug.Log("did you make it here0");
        if (Vector3.Distance(transform.position, TargetPosition.position) <= .05f)
        {
            //Debug.Log("did you make it here");
            if (Mathf.Abs(moveAction.action.ReadValue<Vector2>().x) == 1f)
            {
                //print(moveAction.action.ReadValue<Vector2>().x);
                if (!Physics2D.OverlapCircle(TargetPosition.position + new Vector3(moveAction.action.ReadValue<Vector2>().x, 0f, 0f), .2f, whatStopsMovement))
                {
                    TargetPosition.position += new Vector3(moveAction.action.ReadValue<Vector2>().x, 0f, 0f);
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, moveAction.action.ReadValue<Vector2>(), .5f, whatStopsMovement);
                    if (hit.collider != null)
                    {
                        if (hit.collider.CompareTag("Moveable"))
                        {
                            Vector2 pushDir = (hit.collider.transform.position - transform.position).normalized;
                            pushDir.x = Mathf.Round(pushDir.x);
                            pushDir.y = Mathf.Round(pushDir.y);
                            if (hit.collider.GetComponent<MoveableBlockController>().Push(pushDir))
                            {
                                TargetPosition.position += new Vector3(moveAction.action.ReadValue<Vector2>().x, 0f, 0f);
                            }
                        }
                    }
                }
            }


            // print(moveAction.action.ReadValue<Vector2>());
            if (Mathf.Abs(moveAction.action.ReadValue<Vector2>().y) == 1f)
            {
                if (!Physics2D.OverlapCircle(TargetPosition.position + new Vector3(0f, moveAction.action.ReadValue<Vector2>().y, 0f), .2f, whatStopsMovement))
                {
                    TargetPosition.position += new Vector3(0f, moveAction.action.ReadValue<Vector2>().y, 0f);
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, moveAction.action.ReadValue<Vector2>(), .5f, whatStopsMovement);
                    if (hit.collider != null)
                    {
                        if (hit.collider.CompareTag("Moveable"))
                        {
                            Vector2 pushDir = (hit.collider.transform.position - transform.position);
                            pushDir.x = Mathf.Round(pushDir.x);
                            pushDir.y = Mathf.Round(pushDir.y);
                            if (hit.collider.GetComponent<MoveableBlockController>().Push(pushDir))
                            {
                                TargetPosition.position += new Vector3(0f, moveAction.action.ReadValue<Vector2>().y, 0f);
                            }
                        }
                    }
                }
            }
        }



        if (moveAction.action.ReadValue<Vector2>() != Vector2.zero)
        {
            PlayFootStepSound();
        }
    }

    public GameObject GetLookingAt(float distance, LayerMask layerMask)
    {
        // Debug.LogFormat("[PlayerController] GetLookingAt called.");
        Vector2 facingDir = (TargetPosition.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDir, distance, layerMask);
        if (hit.collider == null)
        {
            // Debug.LogFormat("PlayerController: GetLookingAt raycast hit nothing.");
            return null;
        }
        // Debug.LogFormat("PlayerController: GetLookingAt raycast hit {0}.", hit.collider.gameObject.name);
        return hit.collider.gameObject;
    }

    public GameObject[] GetNearby(float distance, LayerMask layerMask)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, distance, layerMask);
        if (hits.Length == 0)
        {
            return null;
        }
        return Array.ConvertAll(hits, hit => hit.gameObject);
    }

    public IInteractable GetClosestInteractable(float distance, LayerMask layerMask)
    {
        GameObject[] nearbyObjects = GetNearby(distance, layerMask);
        if (nearbyObjects == null || nearbyObjects.Length == 0)
        {
            return null;
        }

        GameObject closestObject = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject obj in nearbyObjects)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestObject = obj;
            }
        }

        if (closestObject != null)
        {
            IInteractable interactable = closestObject.GetComponent<IInteractable>();
            return interactable;
        }

        return null;
    }

    public void CheckInteractionPrompts()
    {
        GameObject LookingAt = GetLookingAt(TOOLTIP_DISTANCE, interactableLayer);
        if (LookingAt == null)
        {
            return;
        }
        Debug.LogFormat("PlayerController: CheckInteractionPrompts found {0}.", LookingAt.gameObject.name);
        IInteractable interactable = LookingAt.GetComponent<IInteractable>();
        if (interactable == null)
        {
            Debug.LogFormat("PlayerController: No IInteractable found on {0}.", LookingAt.gameObject.name);
            return;
        }
        interactable.ShowTooltip(true);
    }

    public void TryInteract()
    {
        GameObject LookingAt = GetLookingAt(INTERACT_DISTANCE, interactableLayer);
        if (LookingAt == null)
        {
            Debug.LogFormat("PlayerController: TryInteract found nothing to interact with.");
            return;
        }
        IInteractable interactable = LookingAt.GetComponent<IInteractable>();
        
        if (interactable == null)
        {
            Debug.LogFormat("PlayerController: No IInteractable found on {0}.", LookingAt.gameObject.name);
            return;
        }

        Debug.LogFormat("PlayerController: Found IInteractable on {0}.", LookingAt.gameObject.name);
        interactable.OnInteract();
    }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    print(moveAction.action.ReadValue<Vector2>());
        //    if (collision.CompareTag("Moveable"))
        //    {
        //        // Calculate direction and start the MoveTowards logic
        //        Vector3 pushDir = (collision.transform.position - transform.position).normalized;
        //        collision.GetComponent<MoveableBlockController>().Push(pushDir);
        //        print("HERE?");
        //    }
        //}


    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogFormat("[PlayerController] Triggered with: {0}", collision.gameObject.name);
        if (collision.gameObject.CompareTag("Item"))
        {
            Debug.LogFormat("PlayerController: Collided with item {0}.", collision.gameObject.name);
            GameManager.Instance.HandleItemPickup(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Door"))
        {
            Debug.LogFormat("PlayerController: Collided with door {0}.", collision.gameObject.name);
            GameManager.Instance.TryOpenDoor(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Mask"))
        {
            Debug.LogFormat("PlayerController: Collided with mask {0}.", collision.gameObject.name);
            GameManager.Instance.HandleItemPickup(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Portal"))
        {
            Debug.LogFormat("PlayerController: Collided with portal {0}.", collision.gameObject.name);
            Portal portal = collision.gameObject.GetComponent<Portal>();
            if (portal != null)
            {
                GameManager.Instance.HandlePortalEntry(collision.gameObject, transform);
            }
            else
            {
                Debug.LogError("PlayerController: Portal component missing on collided portal object.");
            }
        }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, INTERACT_DISTANCE);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, TOOLTIP_DISTANCE);
    }
}
