using System;
using UnityEngine;
using System.Collections;

public class P : MonoBehaviour
{
    [SerializeField, Range(1.0f, 100.0f)] private float _rotateSpeed;
    [SerializeField] private AudioClip _pickUpSound;
    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(0.0f, 0.0f, _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D _kenRB = other.GetComponent<Rigidbody2D>();
            if (_kenRB)
            {
                StartCoroutine(GameBehavior.Instance.PowerUp());
            }
            _source.PlayOneShot(_pickUpSound);
            Destroy(gameObject);
        }
    }
}
