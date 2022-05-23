using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class PlayerController : MonoBehaviour
{
    public float RunSpeed;
    public float JumpForce;
    public float GroundedRadius;
    public float GravityScale;
    public float GlobalGravity;
    public LayerMask WhatIsGround;
    public LayerMask ShootThrough;
    public LayerMask Ladders;
    
    private bool _isGrounded;
    private bool _jump = false;
    private bool _regen = true;
    private bool _canshoot = true;
    private float _originalGravity;
    private float _currHealth;
    private Rigidbody _rigidbody;
    private Vignette _vignette;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _stepSmooth;
    [SerializeField] private float _stepRange;
    [SerializeField] private Transform _groundcheck;
    [SerializeField] private Transform _rayCastLower;
    [SerializeField] private Transform _rayCastUpper;
    [SerializeField] private Transform _gun;
    [SerializeField] private PostProcessVolume _volume;
    [SerializeField] private ParticleSystem _muzzleFlash;

    void Start ()
    {
        _currHealth = _maxHealth;
        _volume.profile.TryGetSettings(out _vignette);
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _originalGravity = GravityScale;
    }
   
    void Update ()
    {
        _isGrounded = false;

        Vector3 xInput = Input.GetAxis("Horizontal") * transform.right * RunSpeed;
        Vector3 zInput = Input.GetAxis("Vertical") * transform.forward * RunSpeed;

        Vector3 move = xInput + zInput;
        float factor = RunSpeed / move.magnitude;
        if (factor < 1)
            move *= factor;
        _rigidbody.velocity = new Vector3(move.x, _rigidbody.velocity.y, move.z);

        Collider[] colliders = Physics.OverlapSphere(_groundcheck.position, GroundedRadius, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				_isGrounded = true;
		}

        if (_currHealth <= 0)
            Destroy(gameObject);

        _vignette.intensity.value = 1 - (_currHealth / _maxHealth);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
        }

        if (Input.GetMouseButton(0) && _canshoot)
            Shoot();

        if (_regen)
            StartCoroutine(_regenerate());
    }

    void FixedUpdate ()
    {
        if (_isGrounded)
            GravityScale = 0;
        Vector3 gravity = GlobalGravity * GravityScale * Vector3.up;
        _rigidbody.AddForce(gravity, ForceMode.Acceleration);

        if (_jump)
        {
            _jump = false;
            GravityScale = 0;

            if (_isGrounded)
                _rigidbody.velocity += JumpForce * transform.up;
        }
        else
        {
            GravityScale = _originalGravity;
        }

        RaycastHit hitLower;
        if (Physics.Raycast(_rayCastLower.position, _rayCastLower.forward, out hitLower, _stepRange))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(_rayCastUpper.position, _rayCastUpper.forward, out hitUpper, _stepRange, ~Ladders))
            {
                _rigidbody.position += new Vector3(0, _stepSmooth, 0f);
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Damage (int dmg)
    {
        _currHealth -= dmg;
    }

    void Shoot()
    {
        _canshoot = false;
        StartCoroutine(CycleRound());
        _muzzleFlash.Play();
        RaycastHit hit;
        Physics.Raycast(_gun.transform.position, _gun.transform.forward, out hit, Mathf.Infinity, ~ShootThrough);

        hit.collider?.gameObject?.GetComponent<DemonStateManager>()?.Damage(10);
    }

    IEnumerator _regenerate()
    {
        _regen = false;
        yield return new WaitForSeconds(1.0f);
        _currHealth += 6.66f;
    }

    IEnumerator CycleRound()
    {
        yield return new WaitForSeconds(0.15f);
        _canshoot = true;
    }
}
