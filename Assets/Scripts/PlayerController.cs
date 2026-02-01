using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveAction;
    public float moveSpeed;
    public Transform TargetPosition;
    public LayerMask whatStopsMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TargetPosition.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
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


            print(moveAction.action.ReadValue<Vector2>());
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
}
