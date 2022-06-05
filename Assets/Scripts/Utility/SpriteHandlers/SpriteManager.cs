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
        _myRenderer.sprite = idleDict[_myCompass.MyDirection];
    }
}
