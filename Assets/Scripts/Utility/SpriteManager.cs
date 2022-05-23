using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyCompass;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private EnemyCompass _myCompass;
    [SerializeField] private Animator _myAnimator;
    [SerializeField] private SpriteRenderer _myRenderer;
    [SerializeField] private DemonStateManager demon;
    private Direction[] directions = new Direction[] {Direction.Fore, Direction.Rear, Direction.Right, Direction.Left, Direction.ForeRight, Direction.ForeLeft, Direction.RearRight, Direction.RearLeft};
    private Dictionary<Direction, Sprite> idleDict = new Dictionary<Direction, Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            idleDict[directions[i]] = idleSprites[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (demon.IsWalking)
        {
            _myAnimator.SetBool("isWalking", true);
            _myAnimator.SetBool("isShooting", false);

            switch(_myCompass._myDirection)
            {
                case Direction.Fore:
                    _myAnimator.SetFloat("walkRotation", 0);
                    _myAnimator.SetFloat("idleRotation", 0);
                    break;
                case Direction.Rear:
                    _myAnimator.SetFloat("walkRotation", 1);
                    _myAnimator.SetFloat("idleRotation", 1);
                    break;
                case Direction.Right:
                    _myAnimator.SetFloat("walkRotation", 2);
                    _myAnimator.SetFloat("idleRotation", 2);
                    break;
                case Direction.Left:
                    _myAnimator.SetFloat("walkRotation", 3);
                    _myAnimator.SetFloat("idleRotation", 3);
                    break;
                case Direction.ForeRight:
                    _myAnimator.SetFloat("walkRotation", 4);
                    _myAnimator.SetFloat("idleRotation", 4);
                    break;
                case Direction.ForeLeft:
                    _myAnimator.SetFloat("walkRotation", 5);
                    _myAnimator.SetFloat("idleRotation", 5);
                    break;
                case Direction.RearRight:
                    _myAnimator.SetFloat("walkRotation", 6);
                    _myAnimator.SetFloat("idleRotation", 6);
                    break;
                case Direction.RearLeft:
                    _myAnimator.SetFloat("walkRotation", 7);
                    _myAnimator.SetFloat("idleRotation", 7);
                    break;
            }
        }
        else
        {
            _myAnimator.SetBool("isWalking", false);
            _myAnimator.SetBool("isShooting", false);

            switch(_myCompass._myDirection)
            {
                case Direction.Fore:
                    _myAnimator.SetFloat("idleRotation", 0);
                    break;
                case Direction.Rear:
                    _myAnimator.SetFloat("idleRotation", 1);
                    break;
                case Direction.Right:
                    _myAnimator.SetFloat("idleRotation", 2);
                    break;
                case Direction.Left:
                    _myAnimator.SetFloat("idleRotation", 3);
                    break;
                case Direction.ForeRight:
                    _myAnimator.SetFloat("idleRotation", 4);
                    break;
                case Direction.ForeLeft:
                    _myAnimator.SetFloat("idleRotation", 5);
                    break;
                case Direction.RearRight:
                    _myAnimator.SetFloat("idleRotation", 6);
                    break;
                case Direction.RearLeft:
                    _myAnimator.SetFloat("idleRotation", 7);
                    break;
            }
        }
    }

    public void Shoot()
    {
        _myAnimator.SetTrigger("shoot");
    }
}
