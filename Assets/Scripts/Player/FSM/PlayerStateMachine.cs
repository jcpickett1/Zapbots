using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStateMachine : MonoBehaviour
{
    // state accessors
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    // input data
    public bool IsRightAttackPressed { get { return _isRightAttackPressed; } set { _isRightAttackPressed = value;}}
    public bool IsLeftAttackPressed { get { return _isLeftAttackPressed; } set { _isLeftAttackPressed = value;}}
    public bool IsMovementPressed { get { return _isMovementPressed; }}
    public bool IsJumpPressed { get { return _isJumpPressed; }}
    public bool IsRunPressed { get { return _isRunPressed; }}
    public bool IsGrounded { get { return _isGrounded; }}

    // player data
    public PlayerDataHandler PlayerData { get { return _playerData; }}
    public Transform RayCastLower { get { return _rayCastLower; }}
    public Transform RayCastUpper { get { return _rayCastUpper; }}
    public LayerMask ShootThrough { get { return _shootThrough; }}
    public Rigidbody MyPhysics { get { return _myPhysics; }}
    public LayerMask Ladders { get { return _ladders; }}

    // context variables
    [SerializeField] private PlayerDataHandler _playerData;
    [SerializeField] private PostProcessVolume _volume;
    [SerializeField] private Transform _rayCastLower;
    [SerializeField] private Transform _rayCastUpper;
    [SerializeField] private Transform _groundcheck;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _shootThrough;
    [SerializeField] private LayerMask _ladders;

    // local data
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    private Rigidbody _myPhysics;
    private Vignette _vignette;
    private bool _isRightAttackPressed = false;
    private bool _isLeftAttackPressed = false;
    private bool _isMovementPressed = false;
    private bool _isJumpPressed = false;
    private bool _isRunPressed = false;
    private bool _isGrounded;
    private float _currHealth;
    private float _maxHealth;


    void Awake()
    {
        // set up variables
        _volume.profile.TryGetSettings(out _vignette);
        _myPhysics = gameObject.GetComponent<Rigidbody>();
        _playerData.LoadPlayerConstants();
        _playerData.LoadPlayerStats();
        _maxHealth = _playerData.MaxHealth;
        _currHealth = _maxHealth;

        // prepare state machine
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    void Update()
    {  
        // variables to force every frame
        _isGrounded = false;

        // check if player is currently grounded
        Collider[] colliders = Physics.OverlapSphere(_groundcheck.position, _playerData.GroundedRadius, _whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
			if (colliders[i].gameObject != gameObject)
				_isGrounded = true;

        // check if player is trying to move
        _isMovementPressed = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) ? true : false;

        // check if player is trying to run
        _isRunPressed = (Input.GetKey(KeyCode.LeftShift)) ? true : false;

        // check if player is trying to attack
        _isRightAttackPressed = Input.GetMouseButton(0) ? true : false;
        _isLeftAttackPressed = Input.GetMouseButton(1) ? true : false;

        // check if player is trying to jump
        if (Input.GetKey(KeyCode.Space) && _isGrounded)
            StartCoroutine(JumpTimeout());
        _isJumpPressed = (Input.GetKey(KeyCode.Space) && (_isJumpPressed || _isGrounded)) ? true : false;

        // regen
        _currHealth += _playerData.RegenRate * Time.deltaTime;

        // bleedout
        _vignette.intensity.value = 1 - (_currHealth / _maxHealth);

        // call update function in substates
        _currentState.UpdateStates();
    }

    public void Damage (int dmg)
    {
        _currHealth -= dmg;
    }

    IEnumerator JumpTimeout()
    {
        yield return new WaitForSeconds(_playerData.JumpTimeout);
        _isJumpPressed = false;
    }
}
