using System.Collections.Generic;

public class PlayerStateFactory
{
    PlayerStateMachine _context;
    enum State { Idle, Walk, Run, Jump, Grounded }
    Dictionary<State, PlayerBaseState> _states = new Dictionary<State, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        _states[State.Grounded] = new PlayerGroundedState(_context, this);
        _states[State.Idle] = new PlayerIdleState(_context, this);
        _states[State.Walk] = new PlayerWalkState(_context, this);
        _states[State.Jump] = new PlayerJumpState(_context, this);
        _states[State.Run] = new PlayerRunState(_context, this);
    }

    public PlayerBaseState Idle(){
        return _states[State.Idle];
    }

    public PlayerBaseState Walk(){
        return _states[State.Walk];
    }

    public PlayerBaseState Run(){
        return _states[State.Run];
    }

    public PlayerBaseState Jump(){
        return _states[State.Jump];
    }

    public PlayerBaseState Grounded(){
        return _states[State.Grounded];
    }
}