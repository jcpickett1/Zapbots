using UnityEngine;
using UnityEngine.AI;

public class DemonHuntState : DemonBaseState
{
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    private Transform _demonTransform;

    public override void EnterState(DemonStateManager demon)
    {
        _navMeshAgent = demon.NavMeshAgent;
        demon.IsWalking = true;

        _player = demon.Player;
        _demonTransform = demon.transform;
    }

    public override void UpdateState(DemonStateManager demon)
    {
        RaycastHit hit;
        Physics.Raycast(demon.Head.transform.position, (_player.position - demon.Head.transform.position), out hit, Mathf.Infinity, Physics.AllLayers);

        if ((_player.position - demon.transform.position).sqrMagnitude >= demon.AimRange * demon.AimRange && demon.HasAttention || hit.collider?.gameObject?.transform != _player)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.destination = _player.position;
            demon.Eyes.position = Vector3.Lerp(demon.Eyes.position, _navMeshAgent.steeringTarget, demon.NavSpeed * Time.deltaTime);
            demon.LastPlayerPosition = _player;
        }
        else if (demon.HasAttention)
        {
            demon.SwitchState(demon.AttackState);
        }
        else
        {
            demon.SwitchState(demon.IdleState);
        }
    }

    public override void OnCollisionEnter(DemonStateManager demon)
    {
        
    }
}
