using UnityEngine;

public abstract class DemonBaseState
{
    public abstract void EnterState(DemonStateManager demon);

    public abstract void UpdateState(DemonStateManager demon);

    public abstract void OnCollisionEnter(DemonStateManager demon);
}
