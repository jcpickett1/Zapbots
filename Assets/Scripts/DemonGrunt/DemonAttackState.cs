using UnityEngine;
using UnityEngine.AI;

public class DemonAttackState : DemonBaseState
{
    private NavMeshAgent _navMeshAgent;
    private Transform _player;

    public override void EnterState(DemonStateManager demon)
    {
        _navMeshAgent = demon.NavMeshAgent;
        demon.IsWalking = false;
        _navMeshAgent.isStopped = true;
        _navMeshAgent.destination = demon.transform.position;
        _player = demon.Player;
    }

    public override void UpdateState(DemonStateManager demon)
    {
        RaycastHit hit;
        Physics.Raycast(demon.Head.transform.position, (_player.position - demon.Head.transform.position), out hit, Mathf.Infinity, ~demon.Invisible);
        if ((_player.position - demon.transform.position).sqrMagnitude >= demon.AimRange * demon.AimRange || hit.collider?.gameObject?.transform != _player)
        {
            demon.SwitchState(demon.HuntState);
        }
        else
        {
            demon.IsWalking = false;
            _navMeshAgent.isStopped = true;
            _navMeshAgent.destination = demon.transform.position;
            demon.Eyes.position = Vector3.Lerp(demon.Eyes.position, _player.position, demon.AimSpeed * Time.deltaTime);
            if ((_player.position - demon.Eyes.position).sqrMagnitude <= 0.16 && demon.CanShoot)
            {
                demon.Shoot();
            }
        }
    }
}
