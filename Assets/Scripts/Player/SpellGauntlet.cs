using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;


public class SpellGauntlet : MonoBehaviour
{
    [SerializeField] private Sprite _relaxedSprite;
    [SerializeField] private Sprite _attackingSprite;
    [SerializeField] private SpriteRenderer _myRenderer;
    [SerializeField] private bool _left = false;

    private Transform _firingLine;
    private PlayerStateMachine _player;
    private SpellData[] _spells;
    private Dictionary<SpellData, GameObject> _spellBases = new Dictionary<SpellData, GameObject>();
    private SpellData _selectedSpell;
    private int _selectedIndex = 0;
    private int _mouseButton;
    private bool _charged = true;
    private bool _canSwap = true;

    public SpellData[] GetSpells { get { return _spells; }}
    public int GetIndex { get { return _selectedIndex; }}

    public void Load()
    {
        _firingLine = transform.Find("Aim");

        _player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStateMachine>();
        SpellBook spellBook = JsonUtility.FromJson<SpellBook>(File.ReadAllText(Application.dataPath + "/Data/Spells.json"));
        _spells = spellBook.SpellsList;
        for(int i = 0; i < _spells.Length; i++)
        {
            GameObject prefabInstance = Instantiate(Resources.Load(_spells[i].SpellType)) as GameObject;
            _spellBases.Add(_spells[i], prefabInstance);
        }
        _selectedSpell = _spells[_selectedIndex];
    }

    public void Shoot()
    {
        _charged = false;
        StartCoroutine(AttackAnimate());
        StartCoroutine(Recharge());

        GameObject projectile = GameObject.Instantiate(_spellBases[_selectedSpell], _firingLine.position, _firingLine.rotation);
        SpriteRenderer projectileSprite = projectile.transform.Find("ProjectileSprite").gameObject.GetComponent<SpriteRenderer>();
        projectileSprite.transform.Rotate(new Vector3(_selectedSpell.Rotation[0], _selectedSpell.Rotation[1], _selectedSpell.Rotation[2]));
        projectile.GetComponent<Rigidbody>().velocity = _player.MyPhysics.velocity;
        ProjectileSpell projectileController = projectile.transform.Find("ProjectileController").gameObject.GetComponent<ProjectileSpell>();
        projectileController.Speed = _selectedSpell.Speed;
        projectileController.Damage = _selectedSpell.Damage;
        projectileController.Element = _selectedSpell.Element;

        projectileSprite.sprite = Resources.Load<Sprite>(_selectedSpell.Filename);
    }

    public void Swap(float scroll)
    {
        _canSwap = false;
        if (scroll > 0)
        {
            _selectedIndex++;
            if (_selectedIndex >= _spells.Length)
                _selectedIndex = 0;
        }
        else if (scroll < 0)
        {
            _selectedIndex--;
            if (_selectedIndex < 0)
                _selectedIndex = _spells.Length - 1;
        }

        _selectedSpell = _spells[_selectedIndex];
        _canSwap = true;
    }

    void Awake()
    {
        Load();

        _mouseButton = 0;
        if (_left)
            _mouseButton = 1;
    }

    void Update()
    {
        bool _shouldFire = (_player.IsLeftAttackPressed && _left || _player.IsRightAttackPressed && !_left);
        if (_shouldFire && _charged)
            Shoot();

        if ((!Input.GetKey(KeyCode.LeftControl) && !_left) || (Input.GetKey(KeyCode.LeftControl) && _left))
        {
            if (_canSwap)
                Swap(Input.mouseScrollDelta.y);
        }
    }

    IEnumerator AttackAnimate()
    {
        _myRenderer.sprite = _attackingSprite;
        yield return new WaitForSeconds(Mathf.Max((_selectedSpell.RechargeTime / 2), 0.25f));
        _myRenderer.sprite = _relaxedSprite;
    }
    
    IEnumerator Recharge()
    {
        yield return new WaitForSeconds(_selectedSpell.RechargeTime);
        _charged = true;
    }
}
