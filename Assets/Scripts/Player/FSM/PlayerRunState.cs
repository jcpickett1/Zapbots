using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        Vector3 xInput = Input.GetAxis("Horizontal") * _ctx.transform.right * _myData.RunSpeed * 2f;
        Vector3 zInput = Input.GetAxis("Vertical") * _ctx.transform.forward * _myData.RunSpeed * 2f;

        Vector3 move = xInput + zInput;
        float factor = _myData.RunSpeed / move.magnitude;

        _ctx.MyPhysics.velocity = new Vector3(move.x, _ctx.MyPhysics.velocity.y, move.z);

        RaycastHit hitLower;
        if (Physics.Raycast(_ctx.RayCastLower.position, _ctx.RayCastLower.forward, out hitLower, _myData.StepRange))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(_ctx.RayCastUpper.position, _ctx.RayCastUpper.forward, out hitUpper, _myData.StepRange, ~_ctx.Ladders))
            {
                _ctx.MyPhysics.position += new Vector3(0, _myData.StepSmooth * Time.deltaTime, 0f);
            }
        }

        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if (_ctx.IsMovementPressed && !_ctx.IsRunPressed)
            SwitchState(_factory.Walk());
        if (!_ctx.IsMovementPressed)
            SwitchState(_factory.Idle());
    }
}
