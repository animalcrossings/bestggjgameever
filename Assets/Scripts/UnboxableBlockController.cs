using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class UnboxableBlockController : MoveableBlockController, IUnboxable
{

    [Header("Data Objects")]
    public BlockData currentBlockData;
    public BlockData unboxStationData;
    public InventoryItemData containedItem;

    [Header("References")]
    public GameObject inventoryItemPrefab;
    public Transform playerTransform;
    public LayerMask groundTileLayer;
    public Sprite emptyBoxSprite;
    public SpriteRenderer spriteRenderer;

    public bool IsUnboxed { get; private set; } = false;

    public void AnimateToPlayer()
    {
        Debug.LogFormat("UnboxableBlock: Animating item {0} to player.", containedItem.id);

        Vector3 ItemSpawnPosition = transform.position;
        // set z to -10 to be in front of everything
        ItemSpawnPosition.z = -10;
        GameObject item = Instantiate(inventoryItemPrefab, ItemSpawnPosition, Quaternion.identity);

        InventoryItem inventoryItem = item.GetComponent<InventoryItem>();
        inventoryItem.Initialize(containedItem);

        inventoryItem.AddComponent<SpriteRenderer>().sprite = containedItem.sprite;

        
        // 2. Get the BezierMover component
        BezierMover mover = item.AddComponent<BezierMover>();
        
        if (mover != null)
        {
            Vector3 start = transform.position;
            Vector3 end = playerTransform.position;
            
            // 3. Calculate a control point dynamically (e.g., 5 units strictly UP from the midpoint)
            Vector3 midPoint = (start + end) / 2;
            Vector3 controlPoint = midPoint + Vector3.up * 5.0f;

            // 4. Call the move. 
            // We use a Lambda expression () => { ... } to handle what happens when it finishes.
            mover.MoveTo(start, end, controlPoint, 1.5f, () => 
            {
                Debug.Log("Item hit the target!");
                InventoryManager.Instance.AddItem(containedItem); 
            });
        }


    }

    // protected override void OnPushCallback(Vector2 targetPosition)
    // {
    //     // Get the block at the target position
    //     Collider2D hitCollider = Physics2D.OverlapCircle(targetPosition, 0.2f, groundTileLayer);
    //     if (hitCollider == null)
    //     {
    //         // Debug.LogFormat("UnboxableBlock: Block pushed to empty tile at {0}, no unboxing.", targetPosition);
    //         return;
    //     }

    //     UnboxStation block = hitCollider.GetComponent<UnboxStation>();
    //     if (block != null && block.unboxStationData.id == unboxStationData.id)
    //     {
    //         // Debug.LogFormat("UnboxableBlock: Block pushed onto unbox station at {0}, unboxing.", targetPosition);
    //         Unbox();   
    //     }
        
    // }

    public void Unbox()
    {
        if (IsUnboxed)
        {
            // Debug.Log("UnboxableBlock: Already unboxed, skipping.");
            return;
        }
        IsUnboxed = true;
        Debug.LogFormat("UnboxableBlock: Unboxed item {0}.", containedItem.id);
        spriteRenderer.sprite = emptyBoxSprite;
        AnimateToPlayer();
    }
}