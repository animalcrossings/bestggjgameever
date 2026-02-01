using UnityEngine;
using System.Collections;
using System; // Required for Action callbacks

public class BezierMover : MonoBehaviour
{
    private Coroutine currentMoveCoroutine;

    // Call this method from any other script
    public void MoveTo(Vector3 startPos, Vector3 endPos, Vector3 controlPos, float duration, Action onComplete = null)
    {
        // Stop any existing movement to prevent conflicts
        if (currentMoveCoroutine != null) StopCoroutine(currentMoveCoroutine);

        currentMoveCoroutine = StartCoroutine(MoveRoutine(startPos, endPos, controlPos, duration, onComplete));
    }

    private IEnumerator MoveRoutine(Vector3 p0, Vector3 p2, Vector3 p1, float duration, Action onComplete)
    {
        float t = 0f;
        
        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            // Standard Quadratic Bezier
            // P = (1-t)^2*P0 + 2(1-t)t*P1 + t^2*P2
            Vector3 position = Mathf.Pow(1 - t, 2) * p0 + 
                               2 * (1 - t) * t * p1 + 
                               Mathf.Pow(t, 2) * p2;

            transform.position = position;

            // Optional: Face the direction of movement
            Vector3 nextPos = Mathf.Pow(1 - (t + 0.01f), 2) * p0 + 
                              2 * (1 - (t + 0.01f)) * (t + 0.01f) * p1 + 
                              Mathf.Pow(t + 0.01f, 2) * p2;
            transform.LookAt(nextPos);

            yield return null;
        }

        transform.position = p2; // Snap to end
        currentMoveCoroutine = null;

        // Trigger the callback if one was provided
        onComplete?.Invoke();
    }
}