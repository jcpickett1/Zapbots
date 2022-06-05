using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyCompass;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private EnemyCompass _myCompass;
    [SerializeField] private Animator _myAnimator;
    private Direction[] directions = new Direction[] {Direction.Fore, Direction.Rear, Direction.Right, Direction.Left, Direction.ForeRight, Direction.ForeLeft, Direction.RearRight, Direction.RearLeft};

    // Update is called once per frame
    void Update()
    {
        switch(_myCompass.MyDirection)
        {
            case Direction.Fore:
                _myAnimator.SetFloat("Rotation", 0);
                break;
            case Direction.Rear:
                _myAnimator.SetFloat("Rotation", 1);
                break;
            case Direction.Right:
                _myAnimator.SetFloat("Rotation", 2);
                break;
            case Direction.Left:
                _myAnimator.SetFloat("Rotation", 3);
                break;
            case Direction.ForeRight:
                _myAnimator.SetFloat("Rotation", 4);
                break;
            case Direction.ForeLeft:
                _myAnimator.SetFloat("Rotation", 5);
                break;
            case Direction.RearRight:
                _myAnimator.SetFloat("Rotation", 6);
                break;
            case Direction.RearLeft:
                _myAnimator.SetFloat("Rotation", 7);
                break;
        }

        if (Input.GetMouseButton(0))
        {
            _myAnimator.SetBool("isWalking", true);
        }
        else
        {
            _myAnimator.SetBool("isWalking", false);
        }
    }

    public void Shoot()
    {
        _myAnimator.SetTrigger("shoot");
    }
}
