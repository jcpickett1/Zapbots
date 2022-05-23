using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        _isRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _currentSubState.EnterState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        // ground gravity to smooth forces
        // Debug.Log(_ctx.MyPhysics.velocity.y);
        // Debug.Log(_ctx.MyPhysics.velocity.y > (_ctx.GlobalGravity * _ctx.GroundedGravity));
        // Debug.Log(_ctx.MyPhysics.velocity.y);// > (_ctx.GlobalGravity * _ctx.GroundedGravity));
        // Debug.Log((_ctx.GlobalGravity * _ctx.GroundedGravity));
        _ctx.MyPhysics.velocity += _ctx.transform.up * _ctx.GlobalGravity * _ctx.GroundedGravity * Time.deltaTime;
        // if (_ctx.MyPhysics.velocity.y > (_ctx.GlobalGravity * _ctx.GroundedGravity))
            // _ctx.MyPhysics.velocity = new Vector3( _ctx.MyPhysics.velocity.x, _ctx.GlobalGravity * _ctx.GroundedGravity * Time.deltaTime, _ctx.MyPhysics.velocity.z);
    }

    public override void ExitState(){}

    public override void InitializeSubState()
    {
        if (!_ctx.IsMovementPressed)
        {
            SetSubState(_factory.Idle());
        }
        else if (_ctx.IsMovementPressed)
        {
            SetSubState(_factory.Walk());
        }
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsJumpPressed)
        {
            SwitchState(_factory.Jump());
        }
    }
}
