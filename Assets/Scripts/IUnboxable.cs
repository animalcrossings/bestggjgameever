using UnityEngine;

public interface IUnboxable
{

    bool IsUnboxed { get; }
    void Unbox();
}
