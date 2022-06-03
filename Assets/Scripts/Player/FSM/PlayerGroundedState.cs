using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        _isRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        _currentSubState.EnterState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        // ground gravity to smooth forces
        _ctx.MyPhysics.velocity += _ctx.transform.up * _myData.GlobalGravity * _myData.GroundedGravity * Time.deltaTime;
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
