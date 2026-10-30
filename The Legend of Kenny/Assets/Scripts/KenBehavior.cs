using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

public class KenBehavior : MonoBehaviour
{
    private Vector2 _direction = Vector2.zero;
    
    [Header("Properties")]
    [SerializeField] float _speed;
    public static bool Crowned;

    [Header("Directions")]
    [SerializeField] KeyCode _leftDirection = KeyCode.A;
    [SerializeField] KeyCode _rightDirection = KeyCode.D;
    [SerializeField] KeyCode _upDirection = KeyCode.W;
    [SerializeField] KeyCode _downDirection = KeyCode.S;
    [SerializeField] KeyCode _attackButton = KeyCode.Space;
    [SerializeField] KeyCode _blockButton = KeyCode.B;
    
    private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _renderer;
    public static bool FaceRight;
    
    [Header("Weapon")]
    [SerializeField] private GameObject _swordPrefab;
    [SerializeField] private Transform _swordSpawnPoint;
    [SerializeField] private Transform _swordParent;
    
    [Header("Projectile)")]
    [HideInInspector] public bool CanBlock;
    [SerializeField] private AudioClip _pickUpSound;
    [SerializeField] private AudioClip _bullet;
    
    [Header("Audio Properties")]
    [SerializeField] private AudioClip _sword;
    private AudioSource _source;
    [SerializeField] private AudioClip _squeak;
    
    [Header("Shield")]
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private Transform _shieldParent;
    private GameObject _shield;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _source = GetComponent<AudioSource>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        _direction = Vector2.zero;

        if (GameBehavior.Instance.State == Mousekeeping.GameState.Play)
        {
            if (Input.GetKey(_leftDirection))
            {
                _direction -= new Vector2(_speed, 0);
                _renderer.flipX = false;
                FaceRight = false;
            }

            if (Input.GetKey(_rightDirection))
            {
                _direction += new Vector2(_speed, 0);
                _renderer.flipX = true;
                FaceRight = true;
            }

            if (Input.GetKey(_upDirection))
            {
                _direction += new Vector2(0, _speed);
            }

            if (Input.GetKey(_downDirection))
            {
                _direction += new Vector2(0, -_speed);
            }
            
            // if (CanShoot && Input.GetKeyDown(_attackButton))
            // {
            //     Instantiate(_projectilePrefab, _swordSpawnPoint.position, _swordParent.rotation, _swordParent);
            // }
        }
    }
    
    void Update()
    {
        if (GameBehavior.Instance.State == Mousekeeping.GameState.GameOver)
        {
            Destroy(gameObject);
        }
        _rb.linearVelocity = _direction.normalized * _speed;
        // if (!CanBlock)
        {
            if (Input.GetKeyDown(_attackButton))
            {
                Instantiate(_swordPrefab, transform.position, transform.rotation, _swordParent);
            }

            if (Input.GetKeyDown(_attackButton))
            {
                _source.clip = _sword;
                _source.Play();
            }
        }
        if (CanBlock && Input.GetKeyDown(_blockButton))
        {
           _shield = Instantiate(_shieldPrefab, transform.position, transform.rotation, transform);
        }
        if (Input.GetKeyUp(_blockButton))
        {
            Destroy(_shield);
            // Debug.Log("Not blocking");
        }

        if (!CanBlock)
        {
            Destroy(_shield);
            // Debug.Log("CANNOT BLOCK");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("projectile"))
        {
            _source.PlayOneShot(_bullet);
            GameBehavior.Instance.KenHealth--;
            Debug.Log(GameBehavior.Instance.KenHealth);
            if (GameBehavior.Instance.KenHealth == 0)
            {
                GameBehavior.Instance.State = Mousekeeping.GameState.GameOver;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            _source.clip = _pickUpSound;
            _source.PlayOneShot(_pickUpSound);
        }
        if (other.CompareTag("Load"))
        {
            SceneManager.LoadScene("BossScene");
        }
        else if (other.CompareTag("Crown"))
        {
            Crowned = true;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
