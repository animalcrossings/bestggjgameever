using UnityEngine;

public class EndLevelBlock : MonoBehaviour, IInteractable
{
    public void OnInteract(PuzzleEntity interactor)
    {
        Debug.Log("Level Completed!");
        GameManager.Instance.CompleteLevel();
    }

    public bool OnBump(PuzzleEntity bumper)
    {
        // No special bump behavior
        return false;
    }
}