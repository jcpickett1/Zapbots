using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStateMachine : MonoBehaviour
{
    // state variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // state accessors
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    // context variables
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _groundedRadius;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpTimeout;
    [SerializeField] private float _gravityScale;
    [SerializeField] private float _globalGravity;
    [SerializeField] private float _groundedGravity;
    [SerializeField] private float _terminalVelocity;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _stepSmooth;
    [SerializeField] private float _stepRange;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _shootThrough;
    [SerializeField] private LayerMask _ladders;
    [SerializeField] private Transform _groundcheck;
    [SerializeField] private Transform _rayCastLower;
    [SerializeField] private Transform _rayCastUpper;
    [SerializeField] private Transform _gun;
    [SerializeField] private PostProcessVolume _volume;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private Camera _firstPerson;
    [SerializeField] private Camera _sideView;
    [SerializeField] private Camera _topDown;

    private bool _isGrounded;
    private bool _isJumpPressed = false;
    private bool _isMovementPressed = false;
    private bool _regen = true;
    private bool _canshoot = true;
    private float _originalGravity;
    private float _currHealth;
    private Rigidbody _myPhysics;
    private Vignette _vignette;

    // getters and setters
    public Rigidbody MyPhysics { get { return _myPhysics; }}
    public bool IsJumpPressed { get { return _isJumpPressed; }}
    public bool IsMovementPressed { get { return _isMovementPressed; }}
    public bool IsGrounded { get { return _isGrounded; }}
    public float RunSpeed { get { return _runSpeed; }}
    public float JumpForce { get { return _jumpForce; }}
    public float OriginalGravity { get { return _originalGravity; }}
    public float GravityScale { get { return _gravityScale; } set { _gravityScale = value; }}
    public float GlobalGravity { get { return _globalGravity; } set { _globalGravity = value; }}
    public float GroundedGravity { get { return _groundedGravity; } set { _groundedGravity = value; }}

    void Awake()
    {
        // set up variables
        _myPhysics = gameObject.GetComponent<Rigidbody>();
        _originalGravity = _gravityScale;

        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    // start is called before the first frame update
    void Start()
    {
        
    }

    // update is called once per frame
    void Update()
    {  
        // variables to force every frame
        _isGrounded = false;

        // check if player is currently grounded
        Collider[] colliders = Physics.OverlapSphere(_groundcheck.position, _groundedRadius, _whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				_isGrounded = true;
		}

        // check if player is trying to move
        _isMovementPressed = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) ? true : false;

        // check if player is trying to jump
        if (Input.GetKey(KeyCode.Space) && _isGrounded)
        {
            StartCoroutine(JumpTimeout());
            _isJumpPressed = true;
        }
        else if (Input.GetKey(KeyCode.Space) && _isJumpPressed)
        {
            _isJumpPressed = true;
        }
        else
        {
            _isJumpPressed = false;
        }

        _currentState.UpdateStates();

        // debug
        if (Input.GetKey(KeyCode.F1))
        {
            _sideView.enabled = false;
            _topDown.enabled = false;
            _firstPerson.enabled = true;
        }
        if (Input.GetKey(KeyCode.F2))
        {
            _sideView.enabled = true;
            _topDown.enabled = false;
            _firstPerson.enabled = false;
        }
        if (Input.GetKey(KeyCode.F3))
        {
            _sideView.enabled = false;
            _topDown.enabled = true;
            _firstPerson.enabled = false;
        }
    }

    IEnumerator JumpTimeout()
    {
        yield return new WaitForSeconds(_jumpTimeout);
        _isJumpPressed = false;
    }
}
