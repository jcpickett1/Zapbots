using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpell : MonoBehaviour
{
    public string Element;
    public float Damage;
    public float Speed;

    [SerializeField] private Rigidbody _myPhysics;

    public ProjectileSpell(float _speed, float _damage, string _element)
    {
        Speed = _speed;
        Damage = _damage;
        Element = _element;
    }

    void Start()
    {
        _myPhysics.velocity += -Speed * transform.forward;   
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider?.gameObject?.GetComponent<DemonStateManager>()?.Damage(Damage);
            Destroy(gameObject.transform.parent.gameObject);
        }
        else if (!collider.CompareTag("Player"))
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
