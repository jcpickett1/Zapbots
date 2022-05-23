using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemonStateManager : MonoBehaviour
{
    #region "States"
    private DemonBaseState _currentState;

    public DemonBaseState LastState;
    public DemonAttackState AttackState = new DemonAttackState();
    public DemonHuntState HuntState = new DemonHuntState();
    public DemonPatrolState PatrolState = new DemonPatrolState();
    public DemonIdleState IdleState = new DemonIdleState();
    #endregion

    #region "Variables"
    [SerializeField] private GameObject _myProjectile;

    public Transform Player;
    public Transform LastPlayerPosition;
    public Transform Eyes;
    public Transform Head;
    public Transform Avatar;
    public Transform CastPoint;
    public NavMeshAgent NavMeshAgent;
    public SpriteManager MyManager;
    public float AimRange;
    public float AimSpeed;
    public float FindSpeed;
    public float NavSpeed;
    public float MaxHealth;
    public float CurrentHealth;
    public bool HasAttention = false;
    public bool IsWalking = false;
    public bool CanShoot = true;
    public bool Spotted = true;
    public LayerMask Invisible;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _currentState = IdleState;
        _currentState.EnterState(this);
        CurrentHealth = MaxHealth;

        // Player = GameObject.Find("Player").transform;
        Eyes.SetParent(null);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        Physics.Raycast(Head.position, (Eyes.position - Head.position), out hit);
        if (hit.collider?.gameObject?.transform == Player)
        {
            HasAttention = true;
            Spotted = true;
        }
        else
        {
            StartCoroutine(LoseTrack());
        }

        _currentState.UpdateState(this);

        transform.LookAt(Eyes.position);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        
        if (CurrentHealth <= 0)
        {
            Destroy(Eyes.gameObject);
            Destroy(gameObject);
        }
    }

    public void SwitchState(DemonBaseState state)
    {
        LastState = _currentState;
        _currentState = state;
        state.EnterState(this);
    }

    public void StartReload()
    {
        StartCoroutine(Reload());
    }

    public void Shoot()
    {
        CanShoot = false;
        MyManager.Shoot();
        GameObject instance = GameObject.Instantiate(_myProjectile, CastPoint.position, transform.rotation);
        instance.GetComponent<DemonProjectile>().player = Player;
        instance.transform.LookAt(Player.position);
        StartCoroutine(Reload());
    }

    public void Damage(float amount)
    {
        CurrentHealth -= amount;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(2.25f);
        CanShoot = true;
    }

    IEnumerator LoseTrack()
    {
        yield return new WaitForSeconds(5f);
        if (!Spotted)
        {
            HasAttention = false;
        }
        else
        {
            HasAttention = true;
            Spotted = false;
        }
    }
}