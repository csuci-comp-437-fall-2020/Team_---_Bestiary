using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerControls Controls;
    public float moveSpeed;
    public float jumpHeight;
    public float dashSpeed;
    public float slidingGravityScale;
    [SerializeField] private float baseGravity;
    [SerializeField] private GameManager gameManager;
    // Delet This
    private GameObject spawner;
    [SerializeField] private GameObject spawnerPrefab;

    private Rigidbody2D _body;
    private SpriteRenderer _sprite;
    private CircleCollider2D _hurtBox;
    private Vector3 _slideJump;

    private Vector2 _lsMove;
    private State _currentState = State.Air;
    private int _jumpsLeft;
    private int _dashesLeft;
    private float _clingTimeLeft;
    private float _floatTimeLeft;
    

    public int extraJumps;
    public int extraDashes;
    public int slamPower;
    public float slamSpeed;
    public float wallClingTime;
    public float floatSpeed;
    public float floatDuration;

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
        Controls = new PlayerControls();

        Controls.Gameplay.A.performed += ctx => Jump();
        Controls.Gameplay.RS.performed += ctx => Jump();
        Controls.Gameplay.LB.performed += ctx => LeftDash();
        Controls.Gameplay.RB.performed += ctx => RightDash();
        Controls.Gameplay.LS.performed += ctx => Slam();
        Controls.Gameplay.LT.performed += ctx => StartHover();
        Controls.Gameplay.LT.canceled += ctx => StopHover();
        Controls.Gameplay.X.performed += ctx => StartHover();
        Controls.Gameplay.X.canceled += ctx => StopHover();
        Controls.Gameplay.Move.performed += ctx => _lsMove = ctx.ReadValue<Vector2>();
        Controls.Gameplay.Move.canceled += ctx => _lsMove = Vector2.zero;
        Controls.Gameplay.Select.performed += ctx => gameManager.ExitGame();
        // Delet This
        Controls.Gameplay.Start.performed += ctx => Spawn();
        Controls.Gameplay.Down.performed += ctx => CycleSlam();
    }

    private void OnEnable()
    {
        Controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        Controls.Gameplay.Disable();
    }

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _hurtBox = GetComponent<CircleCollider2D>();
        Physics2D.gravity = new Vector3(0f, baseGravity, 0f);
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
                    _clingTimeLeft = wallClingTime;
                    _body.gravityScale = 0;
                }
                break;
            case "Slide":
                RestoreJumps();
                _currentState = State.Sliding;
                _slideJump = other.gameObject.transform.rotation * Vector3.up;
                _slideJump *= jumpHeight;
                _body.gravityScale = slidingGravityScale;
                break;
        }
    }

    private void RestoreJumps()
    {
        _hurtBox.enabled = true;
        _body.gravityScale = 1;
        _jumpsLeft = extraJumps;
        _dashesLeft = extraDashes;
        _floatTimeLeft = floatDuration;
        Debug.Log(_floatTimeLeft);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        switch (other.collider.gameObject.tag)
        {
            case "Floor":
            case "Platform":
                _currentState = State.Air;
                break;
            case "Wall":
                _currentState = State.Air;
                _body.gravityScale = 1;
                break;
            case "Slide":
                _currentState = State.Air;
                Physics2D.gravity = new Vector3(0f, baseGravity, 0f);
                break;
        }
    }
    
    // Delet this
    void Spawn()
    {
        if (spawner == null)
        {
            spawner = Instantiate(spawnerPrefab);
            spawner.transform.position = new Vector3(-6, 2.5f, 0);
        }
        else
            spawner.SetActive(true);
    }

    void Jump()
    {
        switch (_currentState)
        {
            case State.Air:
                if (_jumpsLeft > 0f)
                {
                    float height = Mathf.Min(jumpHeight, 1.25f * jumpHeight * _jumpsLeft / extraJumps);
                    _body.velocity = Vector2.zero;
                    _body.AddForce(Vector2.up * height, ForceMode2D.Impulse);
                    _jumpsLeft--;
                }
                break;
            case State.Floating:
                _currentState = State.Air;
                _floatTimeLeft = 0;
                _body.gravityScale = 1;
                if (_jumpsLeft > 0f)
                {
                    float height = Mathf.Min(jumpHeight, 1.25f * jumpHeight * _jumpsLeft / extraJumps);
                    _body.velocity = Vector2.zero;
                    _body.AddForce(Vector2.up * height, ForceMode2D.Impulse);
                    _jumpsLeft--;
                }
                break;
            case State.Grounded:
            case State.Platform:
                _body.velocity = Vector2.zero;
                _body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                break;
            case State.Sliding:
                _body.velocity = new Vector2(_body.velocity.x, 0);
                _body.AddForce(_slideJump, ForceMode2D.Impulse);
                break;
            case State.Clinging:
                _body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                _currentState = State.Air;
                _body.gravityScale = 1;
                break;
        }
    }

    void LeftDash()
    {
        if (_dashesLeft > 0f && _currentState != State.Slamming)
        {
            _body.gravityScale = 1;
            _body.velocity = Vector2.zero;
            _body.AddForce(new Vector2(-dashSpeed, jumpHeight / 3), ForceMode2D.Impulse);
            _dashesLeft--;
        }
    }
    
    void RightDash()
    {
        if (_dashesLeft > 0f && _currentState != State.Slamming)
        {
            _body.gravityScale = 1;
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
        _currentState = State.Air;
        _floatTimeLeft = 0;
        _body.gravityScale = 1;
    }

    void Slam()
    {
        _body.gravityScale = 1;
        switch (slamPower)
        {
            case 1:
                _currentState = State.Slamming;
                _body.velocity = new Vector2(_body.velocity.x, slamSpeed);
                break;
            case 2:
                _currentState = State.Slamming;
                _body.velocity = Vector2.zero;
                _hurtBox.enabled = false;
                break;
            case 3:
                _currentState = State.Slamming;
                _body.AddForce(Vector2.up * (jumpHeight / 1.5f), ForceMode2D.Impulse);
                _body.gravityScale = slidingGravityScale;
                _hurtBox.enabled = false;
                break;
        }
    }

    void CycleSlam()
    {
        slamPower = slamPower % 3 + 1;
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
                _body.gravityScale = 1;
            }
        }
        else if (_currentState == State.Floating)
        {
            _floatTimeLeft -= Time.deltaTime;
            if (_floatTimeLeft < 0)
            {
                _currentState = State.Air;
                _body.gravityScale = 1;
            }
        }
    }
}