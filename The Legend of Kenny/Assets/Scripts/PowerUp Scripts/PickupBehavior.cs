using System;
using UnityEngine;
using System.Collections;

public class P : MonoBehaviour
{
    [SerializeField, Range(1.0f, 100.0f)] private float _rotateSpeed;
    // private AudioSource _source;

    private void Start()
    {
        // _source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(0.0f, 0.0f, _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // _source.Play();
            StartCoroutine(GameBehavior.Instance.PowerUp());
            Destroy(gameObject);
        }
    }
}
