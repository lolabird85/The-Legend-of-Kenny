using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField, Range(1.0f, 50.0f)] float _projectileSpeed = 10.0f;
    private int _hitNumber = 0;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Vector2 direction = Random.onUnitCircle;
        _rb.AddForce(direction * _projectileSpeed, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (GameBehavior.Instance.State == Mousekeeping.GameState.Play)
        {
            _rb.simulated = true;
        }
        else
        {
            _rb.simulated = false;
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            _hitNumber++;
            if (_hitNumber >= 3)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }
}
