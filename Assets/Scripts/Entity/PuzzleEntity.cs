using UnityEngine;
using System.Collections;

public abstract class PuzzleEntity : MonoBehaviour
{
    [Header("State")]
    public Vector2Int GridPosition;
    public bool IsMoving { get; private set; }

    [Header("Config")]
    public PuzzleEntityData data; 

    public virtual void SnapToGrid(Vector2Int pos)
    {
        GridPosition = pos;
        transform.position = new Vector3(pos.x, pos.y, 0);
    }

    public virtual IEnumerator MoveRoutine(Vector2Int targetPos, float duration)
    {
        IsMoving = true;
        Vector3 start = transform.position;
        Vector3 end = new Vector3(targetPos.x, targetPos.y, 0);
        float elapsed = 0;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        GridPosition = targetPos;
        IsMoving = false;
        
        OnMoveComplete();
    }

    protected virtual void OnMoveComplete() { }
}