using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

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
        // print(TargetPosition.position);
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
        }
            if (Mathf.Abs(moveAction.action.ReadValue<Vector2>().y) == 1f)
            {
                if (!Physics2D.OverlapCircle(TargetPosition.position + new Vector3(0f, moveAction.action.ReadValue<Vector2>().y, 0f), .2f, whatStopsMovement))
                {
                    TargetPosition.position += new Vector3(0f, moveAction.action.ReadValue<Vector2>().y, 0f);
                }
            }
        }
    }



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

        double a = 0.0;

        float b = (float)a;
    }
}
