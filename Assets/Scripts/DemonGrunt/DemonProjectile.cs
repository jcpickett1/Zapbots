using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonProjectile : MonoBehaviour
{
    private Rigidbody myPhysics;
    public Transform player;
    public int DamageAmount;
    bool travelling;

    // Start is called before the first frame update
    void Start()
    {
        myPhysics = gameObject.GetComponent<Rigidbody>();
        travelling = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (travelling)
        {
            myPhysics.velocity = 1.5f * transform.forward;
        }

        if (transform.InverseTransformPoint(player.position).z > 0)
        {
            Vector3 heading = (player.position - transform.position).normalized;
            myPhysics.velocity += heading;
            transform.forward = Vector3.Lerp(transform.forward, myPhysics.velocity.normalized, Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject == player.gameObject)
            player.gameObject.GetComponent<PlayerStateMachine>().Damage(DamageAmount);

        Destroy(gameObject);
    }
}
