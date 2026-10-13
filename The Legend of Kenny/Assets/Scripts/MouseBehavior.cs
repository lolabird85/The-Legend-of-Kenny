using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class MouseBehavior : MonoBehaviour
{
    [Header("Mouse Properties")] [SerializeField]
    private float _launchForce = 5.0f;
    private Rigidbody2D _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rb.simulated = GameBehavior.Instance.State == Mousekeeping.GameState.Play;
    }

    private void FixedUpdate()
    {
        RunMouse();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            Destroy(gameObject);
        }
    }

    public void RunMouse()
    {
        Vector2 direction = Random.onUnitCircle;
        _rb.AddForce(direction * _launchForce, ForceMode2D.Impulse);
    }

    void OnDestroy()
    {
        GameBehavior.Instance.MouseDeath++;
        Debug.Log(GameBehavior.Instance.MouseDeath);
    }
}
