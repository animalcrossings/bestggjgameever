using TMPro;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.UI.Image;
using System.Linq;

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
        }
    }
    public bool Push(Vector2 direction)
    {
        if (isMoving) return false;

        Vector2 newTarget = targetPos + direction;

        if (!IsTileBlocked(newTarget, direction))
        {
            targetPos = newTarget;
            isMoving = true;
            return true; 
        }
        return false; 
    }

    bool IsTileBlocked(Vector2 pos, Vector2 dir)
    {
        if (!Physics2D.OverlapCircle(pos, 0.2f, whatStopsMovement)){
            return false;
        }

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, .5f, whatStopsMovement);
        RaycastHit2D[] hitsList = Physics2D.RaycastAll(transform.position, dir, .5f);
        foreach (RaycastHit2D hit in hitsList)
        {
            if (hit.collider.CompareTag("Teleporter"))
            {
                transform.position = hit.collider.GetComponent<PortalController>().blockTPblock.transform.position;
                targetPos = hit.collider.GetComponent<PortalController>().blockTPblock.transform.position;
                return true;
            }
            //if (hit.collider != null)
            //{
            //    print("here?");
            //    print(hit.collider.tag);
            //    if (hit.collider.CompareTag("Teleporter"))
            //    {
            //        print("here???");
            //        transform.position = hit.collider.GetComponent<PortalController>().blockTPblock.transform.position;
            //        targetPos = hit.collider.GetComponent<PortalController>().blockTPblock.transform.position;
            //    }
            //    return true;
            //}
        }
        return true;

    }

}
