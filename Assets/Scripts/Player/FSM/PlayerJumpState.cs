using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        _isRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        if (!_ctx.IsJumpPressed)
            HandleFall();
    }

    public override void ExitState(){}

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if (_ctx.IsGrounded && !_ctx.IsJumpPressed)
            SwitchState(_factory.Grounded());
    }

    void HandleJump()
    {
        _ctx.GravityScale = 0;
        _ctx.MyPhysics.velocity += _ctx.JumpForce * _ctx.transform.up;
    }

    void HandleFall()
    {
        _ctx.GravityScale = _ctx.OriginalGravity;
        _ctx.MyPhysics.velocity += new Vector3( 0, _ctx.GlobalGravity * _ctx.GravityScale * Time.deltaTime, 0);
    }
}
