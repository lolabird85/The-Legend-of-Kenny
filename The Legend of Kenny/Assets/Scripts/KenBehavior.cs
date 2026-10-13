using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class KenBehavior : MonoBehaviour
{
    private Vector2 _direction = Vector2.zero;
    
    [SerializeField] float _speed;
    public static bool Crowned;

    [Header("Directions")]
    [SerializeField] KeyCode _leftDirection = KeyCode.A;
    [SerializeField] KeyCode _rightDirection = KeyCode.D;
    [SerializeField] KeyCode _upDirection = KeyCode.W;
    [SerializeField] KeyCode _downDirection = KeyCode.S;
    [SerializeField] KeyCode _attackButton = KeyCode.Space;
    
    private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _renderer;
    public static bool FaceRight;
    
    [Header("Weapon")]
    [SerializeField] private GameObject _swordPrefab;
    [SerializeField] private Transform _swordSpawnPoint;
    [SerializeField] private Transform _swordParent;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _direction.normalized * _speed;
        if (Input.GetKey(_attackButton))
        {
            Instantiate(_swordPrefab, _swordSpawnPoint.position, _swordParent.rotation, _swordParent);
        }
    }
    
    void Update()
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
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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
