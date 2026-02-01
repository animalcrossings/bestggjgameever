using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveAction;
    public float moveSpeed;
    public Transform TargetPosition;
    public LayerMask whatStopsMovement;
    public float teleportCooldownTime;
    private float teleportTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TargetPosition.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        // stun the player for a moment after teleporting so they can adjust to the fact that they teleported
        if (teleportTimer > 0)
        {
            teleportTimer -= Time.deltaTime;
            return;
        }

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

                        if (hit.collider.CompareTag("Teleporter"))
                        {
                            transform.position = hit.collider.GetComponent<PortalController>().playerTPblock.transform.position;
                            TargetPosition.position = hit.collider.GetComponent<PortalController>().playerTPblock.transform.position;
                            hit.collider.GetComponent<PortalController>().cameraTP();
                            teleportTimer = teleportCooldownTime;
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

                        if (hit.collider.CompareTag("Teleporter"))
                        {
                            transform.position = hit.collider.GetComponent<PortalController>().playerTPblock.transform.position;
                            TargetPosition.position = hit.collider.GetComponent<PortalController>().playerTPblock.transform.position;
                            hit.collider.GetComponent<PortalController>().cameraTP();
                            teleportTimer = teleportCooldownTime;
                        }
                    }
                }
            }
        }
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
            GameManager.Instance.HandleMaskPickup(collision.gameObject);
        }
    }
}
