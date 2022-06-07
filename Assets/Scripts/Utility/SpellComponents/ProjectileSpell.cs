using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpell : SpellSeed
{
    public string Element;
    public float Damage;
    public float Speed;
    public LayerMask[] Masks;

    [SerializeField] private Rigidbody _myPhysics;


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

    public override void Rig(SpellData spell)
    {
        SpriteRenderer projectileSprite = this.transform.parent.Find("ProjectileSprite").gameObject.GetComponent<SpriteRenderer>();
        ProjectileSpell projectileController = this.transform.parent.Find("ProjectileController").gameObject.GetComponent<ProjectileSpell>();

        projectileSprite.transform.Rotate(new Vector3(spell.Rotation[0], spell.Rotation[1], spell.Rotation[2]));
        projectileController.Speed = spell.Speed;
        projectileController.Damage = spell.Damage;
        projectileController.Element = spell.Element;
        projectileSprite.sprite = Resources.Load<Sprite>(spell.Filename);
    }
}
