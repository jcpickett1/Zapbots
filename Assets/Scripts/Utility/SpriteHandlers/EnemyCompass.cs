using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCompass : MonoBehaviour
{
    public enum Direction {Fore, Rear, Right, Left, ForeRight, ForeLeft, RearRight, RearLeft, Far};
    public Direction MyDirection = Direction.Fore;

    private Transform _player;
    private Transform _myRenderer;
    private bool _active = false;


    void Start()
    {
        _player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (_active)
        {
            // Get vector pointing to player, ignoring any altitude differences
            Vector3 playerCenter = new Vector3(_player.position.x, 0, _player.position.z);
            Vector3 myCenter = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 direction = playerCenter - myCenter;

            if (direction.sqrMagnitude <= 62500)
            {
                float angle = Vector3.SignedAngle(transform.forward, direction, transform.up);

                if (angle < 22.5f && angle > -22.5f)
                    MyDirection = Direction.Fore;
                else if (angle < 67.5f && angle > 22.5f)
                    MyDirection = Direction.ForeRight;
                else if (angle < 112.5f && angle > 67.5f)
                    MyDirection = Direction.Right;
                else if (angle < 157.5f && angle > 112.5f)
                    MyDirection = Direction.RearRight;
                else if (angle < -157.5f)
                    MyDirection = Direction.Rear;
                else if (angle > 157.5f)
                    MyDirection = Direction.Rear;
                else if (angle < -112.5f && angle > -157.5f)
                    MyDirection = Direction.RearLeft;
                else if (angle < -67.5f && angle > -112.5f)
                    MyDirection = Direction.Left;
                else if (angle < -22.5f && angle > -67.5f)
                    MyDirection = Direction.ForeLeft;
            }
            else
            {
                MyDirection = Direction.Far;
            }
        }
    }

    void OnBecameVisible()
    {
        _active = true;
    }

    void OnBecameInvisible()
    {
        _active = false;
    }
}
