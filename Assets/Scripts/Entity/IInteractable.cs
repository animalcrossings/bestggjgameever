using UnityEngine;

public interface IInteractable
{
    void OnInteract(PuzzleEntity interactor);
    bool OnBump(PuzzleEntity bumper);
    
}
