using UnityEngine;

public class DemonIdleState : DemonBaseState
{
    private Transform _player;    

    public override void EnterState(DemonStateManager demon)
    {
        _player = GameObject.Find("Player").transform;
    }

    public override void UpdateState(DemonStateManager demon)
    {
        RaycastHit hit;
        Physics.Raycast(demon.Head.transform.position, (_player.position - demon.Head.transform.position), out hit);

        if (hit.collider?.gameObject?.transform == _player)
        {
            if (demon.HasAttention)
            {
                if ((_player.position - demon.transform.position).sqrMagnitude >= demon.AimRange * demon.AimRange)
                {
                    demon.SwitchState(demon.HuntState);
                }
                else
                {
                    demon.SwitchState(demon.AttackState);
                }
            }
            else
            {
                demon.Eyes.position = Vector3.Lerp(demon.Eyes.position, _player.position, demon.FindSpeed * Time.deltaTime);
                if ((_player.position - demon.Eyes.position).sqrMagnitude <= 0.0625)
                    demon.HasAttention = true;
            }
        }
    }

    public override void OnCollisionEnter(DemonStateManager demon)
    {
        
    }
}
