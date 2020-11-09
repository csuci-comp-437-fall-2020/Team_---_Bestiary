using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;
    public float wallKickMultiplier;
    public float dashSpeed;
    public float slamSpeed;
    public float floatSpeed;
    public float slamGravityScale;
    public float slidingGravityScale;
    [SerializeField] private GameManager gameManager;
    
    private Rigidbody2D _body;
    private SpriteRenderer _sprite;
    [SerializeField] private CircleCollider2D hurtBox;

    private PlayerControls _controls;
    private PlayerManager _manager;
    
    private Vector2 _lsMove;
    private State _currentState = State.Air;
    private Vector3 _slideJump;
    private int _jumpsLeft;
    private int _dashesLeft;
    private float _clingTimeLeft;
    private float _floatTimeLeft;

    [HideInInspector] public Vector2 rsMove;
    
    [SerializeField] private PlayerEffects playerEffects;

    enum State
    {
        Air,
        Floating,
        Slamming,
        Grounded,
        Platform,
        Sliding,
        Clinging
    }

    private void Awake()
    {
        _controls = new PlayerControls();

        _controls.Gameplay.A.performed += ctx => Jump();
        _controls.Gameplay.RS.performed += ctx => Jump();
        _controls.Gameplay.LB.performed += ctx => LeftDash();
        _controls.Gameplay.RB.performed += ctx => RightDash();
        _controls.Gameplay.LS.performed += ctx => Slam();
        _controls.Gameplay.LT.performed += ctx => StartHover();
        _controls.Gameplay.LT.canceled += ctx => StopHover();
        _controls.Gameplay.X.performed += ctx => StartHover();
        _controls.Gameplay.X.canceled += ctx => StopHover();
        _controls.Gameplay.Move.performed += ctx => _lsMove = ctx.ReadValue<Vector2>();
        _controls.Gameplay.Move.canceled += ctx => _lsMove = Vector2.zero;
        _controls.Gameplay.Aim.performed += ctx => rsMove = ctx.ReadValue<Vector2>().normalized;
        _controls.Gameplay.Aim.canceled += ctx => rsMove = Vector2.zero;
        _controls.Gameplay.Select.performed += ctx => gameManager.ExitGame();
        _controls.Gameplay.Right.performed += ctx => _manager.CycleRightMask();
        _controls.Gameplay.Down.performed += ctx => CycleSlam();
    }

    private IEnumerator PauseControls(float duration)
    {
        _controls.Gameplay.Disable();
        yield return new WaitForSeconds(duration);
        _controls.Gameplay.Enable();
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _controls.Gameplay.Disable();
    }

    void Start()
    {
        StartCoroutine(PauseControls(1.5f));
        _body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        hurtBox = GetComponent<CircleCollider2D>();
        _manager = GetComponent<PlayerManager>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.collider.gameObject.tag)
        {
            case "Floor":
                RestoreJumps();
                _currentState = State.Grounded;
                break;
            case "Platform":
                RestoreJumps();
                _currentState = State.Platform;
                break;
            case "Wall":
                if (_currentState == State.Air || _currentState == State.Floating)
                {
                    RestoreJumps();
                    _currentState = State.Clinging;
                    _body.velocity = Vector2.zero;
                    _clingTimeLeft = playerEffects.wallClingTime;
                    _body.gravityScale = 0;
                }
                break;
            case "Slide":
                RestoreJumps();
                _currentState = State.Sliding;
                _slideJump = other.gameObject.transform.rotation * Vector3.up;
                _slideJump *= jumpHeight;
                _body.gravityScale *= slidingGravityScale;
                break;
        }
    }

    private void RestoreJumps()
    {
        hurtBox.enabled = true;
        _body.gravityScale = playerEffects.gravityScale;
        _jumpsLeft = playerEffects.extraJumps;
        _dashesLeft = playerEffects.extraDashes;
        _floatTimeLeft = playerEffects.floatDuration;
    }
    
    void OnCollisionExit2D(Collision2D other)
    {
        switch (other.collider.gameObject.tag)
        {
            case "Floor":
            case "Platform":
                _currentState = State.Air;
                break;
        }
    }

    void Jump()
    {
        Debug.Log(_currentState);
        switch (_currentState)
        {
            case State.Air:
                if (_jumpsLeft > 0)
                {
                    float height = Mathf.Min(jumpHeight, 1.25f * jumpHeight * _jumpsLeft / playerEffects.extraJumps);
                    _body.velocity = Vector2.zero;
                    _body.AddForce(Vector2.up * height, ForceMode2D.Impulse);
                    _jumpsLeft--;
                }
                break;
            case State.Floating:
                _currentState = State.Air;
                _floatTimeLeft = 0;
                _body.gravityScale = playerEffects.gravityScale;
                if (_jumpsLeft > 0)
                {
                    float height = Mathf.Min(jumpHeight, 1.25f * jumpHeight * _jumpsLeft / playerEffects.extraJumps);
                    _body.velocity = Vector2.zero;
                    _body.AddForce(Vector2.up * height, ForceMode2D.Impulse);
                    _jumpsLeft--;
                }
                break;
            case State.Grounded:
            case State.Platform:
                _currentState = State.Air;
                _body.velocity = Vector2.zero;
                _body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                break;
            case State.Sliding:
                _currentState = State.Air;
                _body.velocity = new Vector2(_body.velocity.x, 0);
                _body.AddForce(_slideJump, ForceMode2D.Impulse);
                _body.gravityScale = playerEffects.gravityScale;
                break;
            case State.Clinging:
                _currentState = State.Air;
                _body.AddForce(rsMove * (jumpHeight * wallKickMultiplier), ForceMode2D.Impulse);
                _body.gravityScale = playerEffects.gravityScale;
                break;
        }
    }

    void LeftDash()
    {
        if (_dashesLeft > 0 && _currentState != State.Slamming)
        {
            _body.gravityScale = playerEffects.gravityScale;
            _body.velocity = Vector2.zero;
            _body.AddForce(new Vector2(-dashSpeed, jumpHeight / 3), ForceMode2D.Impulse);
            _dashesLeft--;
        }
    }
    
    void RightDash()
    {
        if (_dashesLeft > 0 && _currentState != State.Slamming)
        {
            _body.gravityScale = playerEffects.gravityScale;
            _body.velocity = Vector2.zero;
            _body.AddForce(new Vector2(dashSpeed, jumpHeight / 3), ForceMode2D.Impulse);
            _dashesLeft--;
        }
    }

    void StartHover()
    {
        if (_floatTimeLeft > 0 && _currentState == State.Air)
        {
            _currentState = State.Floating;
            _body.velocity = new Vector2(_body.velocity.x, floatSpeed);
            _body.gravityScale = 0;
        }
    }

    void StopHover()
    {
        if (_currentState == State.Floating)
        {
            _currentState = State.Air;
            _floatTimeLeft = 0;
            _body.gravityScale = playerEffects.gravityScale;
        }
    }

    void Slam()
    {
        if (_currentState == State.Air || _currentState == State.Floating)
        {
            switch (playerEffects.slamEquipped)
            {
                case 1:
                    _currentState = State.Slamming;
                    _body.velocity = new Vector2(_body.velocity.x, slamSpeed);
                    break;
                case 2:
                    _currentState = State.Slamming;
                    _body.velocity = Vector2.zero;
                    hurtBox.enabled = false;
                    break;
                case 3:
                    _currentState = State.Slamming;
                    _body.AddForce(Vector2.up * (jumpHeight / 1.5f), ForceMode2D.Impulse);
                    _body.gravityScale *= slamGravityScale;
                    hurtBox.enabled = false;
                    break;
            }
        }
    }

    void CycleSlam()
    {
        if (playerEffects.slamPower > 0)
            playerEffects.slamEquipped = playerEffects.slamEquipped % playerEffects.slamPower + 1;
    }

    void Update()
    {
        if (_currentState != State.Sliding && _currentState != State.Slamming)
        {
            float deltaX = _lsMove.x * moveSpeed * Time.deltaTime;
            Vector2 movement = new Vector2(deltaX, _body.velocity.y);
            if (_currentState == State.Grounded || Mathf.Abs(_body.velocity.x) < Mathf.Abs(deltaX))
            {
                movement = new Vector2(deltaX, _body.velocity.y);
                _body.velocity = movement;
            }
            if (movement.x != 0)
                _sprite.flipX = movement.x < 0;
        }
        
        if (_currentState == State.Clinging)
        {
            _clingTimeLeft -= Time.deltaTime;
            if (_clingTimeLeft < 0)
            {
                _currentState = State.Air;
                _body.gravityScale = playerEffects.gravityScale;
            }
        }
        else if (_currentState == State.Floating)
        {
            _floatTimeLeft -= Time.deltaTime;
            if (_floatTimeLeft < 0)
            {
                _currentState = State.Air;
                _body.gravityScale = playerEffects.gravityScale;
            }
        }
    }
}