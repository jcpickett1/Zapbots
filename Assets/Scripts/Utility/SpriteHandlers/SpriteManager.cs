using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyCompass;


public class SpriteManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _myRenderer;
    [SerializeField] private EnemyCompass _myCompass;
    [SerializeField] private Sprite[] idleSprites;

    private Direction[] directions = new Direction[] { Direction.Fore, Direction.ForeRight, Direction.Right, Direction.RearRight, Direction.Rear, Direction.RearLeft, Direction.Left, Direction.ForeLeft};
    private Dictionary<Direction, Sprite> idleDict = new Dictionary<Direction, Sprite>();
    private Direction _lastDirection;
    private bool _active = false;


    void Start()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            idleDict[directions[i]] = idleSprites[i];
        }
    }

    void Update()
    {
        if (_active)
        {
            if (_lastDirection != _myCompass.MyDirection)
            {
                _lastDirection = _myCompass.MyDirection;
                if (_lastDirection == Direction.Far)
                {
                    _myRenderer.enabled = false;
                }
                else
                {
                    _myRenderer.enabled = true;
                    _myRenderer.sprite = idleDict[_lastDirection];
                }
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
