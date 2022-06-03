using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float _originalGravity = 3;
    private float _myGravity = 3;

    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        _isRootState = true;
    }

    public override void EnterState()
    {
        _originalGravity = _myData.GravityScale;
        InitializeSubState();
        HandleJump();
    }

    public override void UpdateState()
    {
        Vector3 xInput = Input.GetAxis("Horizontal") * _ctx.transform.right * _myData.RunSpeed;
        Vector3 zInput = Input.GetAxis("Vertical") * _ctx.transform.forward * _myData.RunSpeed;

        Vector3 move = xInput + zInput;
        float factor = _myData.RunSpeed / move.magnitude;
        if (factor < 1)
            move *= factor;
        _ctx.MyPhysics.velocity += new Vector3(move.x * Time.deltaTime, 0, move.z * Time.deltaTime);

        if (!_ctx.IsJumpPressed)
            HandleFall();

        CheckSwitchStates();
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
        _myGravity = 0;
        _ctx.MyPhysics.velocity += _myData.JumpForce * _ctx.transform.up;
    }

    void HandleFall()
    {
        _myGravity = _originalGravity;
        _ctx.MyPhysics.velocity += new Vector3( 0, _myData.GlobalGravity * _myGravity * Time.deltaTime, 0);
    }
}
