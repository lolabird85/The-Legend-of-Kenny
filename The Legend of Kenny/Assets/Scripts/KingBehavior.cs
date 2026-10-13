using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KingBehavior : MonoBehaviour
{
    private Vector2 _direction = Vector2.zero;
    
    [SerializeField] float _speed;

    [Header("Directions")]
    [SerializeField] KeyCode _leftDirection = KeyCode.A;
    [SerializeField] KeyCode _rightDirection = KeyCode.D;
    [SerializeField] KeyCode _upDirection = KeyCode.W;
    [SerializeField] KeyCode _downDirection = KeyCode.S;
    
    private Rigidbody2D _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _direction.normalized * _speed;
    }
    
    void Update()
    {
        _direction = Vector2.zero;
        
        if (Input.GetKey(_leftDirection))
        {
            _direction -= new Vector2(_speed, 0);
        }

        if (Input.GetKey(_rightDirection))
        {
            _direction += new Vector2(_speed, 0);
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Load"))
        {
            SceneManager.LoadScene("Win");
        }
    }
}
