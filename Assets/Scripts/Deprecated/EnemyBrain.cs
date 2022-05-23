using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    private Transform _player;
    private Vector3 _lastPlayerPosition;
    private Transform home;
    private bool hasAttention;
    private bool canShoot = true;

    public Transform _avatar;
    public Transform myEyes;
    public float aimRange;
    public float aimSpeed;
    public float findSpeed;
    public float navSpeed;
    public bool canLog = true;
    public bool isWalking = false;

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private SpriteManager _myManager;
    [SerializeField] private GameObject _myProjectile;

    // Start is called before the first frame update
    void Start()
    {
        home = transform;
        _player = GameObject.Find("Player").transform;
        myEyes.SetParent(null);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, (_player.position - transform.position), out hit);

        if (hit.collider?.gameObject?.transform == _player)
        {
            if (hasAttention)
            {
                if ((_player.position - transform.position).sqrMagnitude >= aimRange * aimRange)
                {
                    isWalking = true;
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = _player.position;
                    Vector3 step = navMeshAgent.steeringTarget - transform.position;
                    myEyes.position = Vector3.Lerp(myEyes.position, navMeshAgent.steeringTarget, navSpeed * Time.deltaTime);
                    _lastPlayerPosition = _player.position;
                }
                else
                {
                    isWalking = false;
                    navMeshAgent.isStopped = true;
                    navMeshAgent.destination = transform.position;
                    myEyes.position = Vector3.Lerp(myEyes.position, _player.position, aimSpeed * Time.deltaTime);
                    if ((_player.position - myEyes.position).sqrMagnitude <= 0.16 && canShoot)
                    {
                        Shoot();
                    }
                }
            }
            else
            {
                myEyes.position = Vector3.Lerp(myEyes.position, _player.position, findSpeed * Time.deltaTime);
                if ((_player.position - myEyes.position).sqrMagnitude <= 0.0625)
                    hasAttention = true;
            }
        }
        else
        {
            hasAttention = false;
            Vector3 myXZ = new Vector3(transform.position.x, 0, transform.position.z);
            if ((navMeshAgent.destination - myXZ).sqrMagnitude <= 0.309)
            {
                isWalking = false;
                navMeshAgent.isStopped = true;
            }
            if (_lastPlayerPosition != null)
            {
                isWalking = true;
                navMeshAgent.isStopped = false;
                navMeshAgent.destination = _lastPlayerPosition;
                myEyes.position = Vector3.Lerp(myEyes.position, navMeshAgent.steeringTarget, 10 * Time.deltaTime);
            }
            else
            {
                navMeshAgent.destination = home.position;
            }
        }

        _avatar.LookAt(myEyes);
        _avatar.rotation = Quaternion.Euler(0f, _avatar.rotation.eulerAngles.y, 0f);
    }

    void Shoot()
    {
        canShoot = false;
        _myManager.Shoot();
        GameObject.Instantiate(_myProjectile, transform.position + transform.forward, transform.rotation);
        StartCoroutine(Reload());
    }

    IEnumerator ThrottledLog(string info)
    {
        if (canLog)
            // log
        yield return new WaitForSeconds(0.0f);
        canLog = true;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(2.25f);
        canShoot = true;
    }
}