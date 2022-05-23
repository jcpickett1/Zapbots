using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        Vector3 xInput = Input.GetAxis("Horizontal") * _ctx.transform.right * _ctx.RunSpeed;
        Vector3 zInput = Input.GetAxis("Vertical") * _ctx.transform.forward * _ctx.RunSpeed;

        Vector3 move = xInput + zInput;
        float factor = _ctx.RunSpeed / move.magnitude;
        if (factor < 1)
            move *= factor;
        _ctx.MyPhysics.velocity = new Vector3(move.x, _ctx.MyPhysics.velocity.y, move.z);

        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if (!_ctx.IsMovementPressed)
            SwitchState(_factory.Idle());
    }
}
