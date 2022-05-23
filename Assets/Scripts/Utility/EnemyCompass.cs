using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCompass : MonoBehaviour
{
    private Transform _player;
    public enum Direction {Fore, Rear, Right, Left, ForeRight, ForeLeft, RearRight, RearLeft};
    public Direction _myDirection = Direction.Fore;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Get vector pointing to player, ignoring any altitude differences
        Vector3 playerCenter = new Vector3(_player.position.x, 0, _player.position.z);
        Vector3 myCenter = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 direction = playerCenter - myCenter;
        float angle = Vector3.SignedAngle(transform.forward, direction, transform.up);


        if (angle < 22.5f && angle > -22.5f)
            _myDirection = Direction.Fore;
        else if (angle < 67.5f && angle > 22.5f)
            _myDirection = Direction.ForeRight;
        else if (angle < 112.5f && angle > 67.5f)
            _myDirection = Direction.Right;
        else if (angle < 157.5f && angle > 112.5f)
            _myDirection = Direction.RearRight;
        else if (angle < -157.5f)
            _myDirection = Direction.Rear;
        else if (angle > 157.5f)
            _myDirection = Direction.Rear;
        else if (angle < -112.5f && angle > -157.5f)
            _myDirection = Direction.RearLeft;
        else if (angle < -67.5f && angle > -112.5f)
            _myDirection = Direction.Left;
        else if (angle < -22.5f && angle > -67.5f)
            _myDirection = Direction.ForeLeft;
    }
}
