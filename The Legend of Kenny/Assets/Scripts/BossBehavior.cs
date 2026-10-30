using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class BossBehavior : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _launchForce = 5.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float _steepnessThreshold = 0.25f;
    private Transform _crownPosition;
    [SerializeField] public static int BossHealth = 100;
    private AudioSource _source;
    [SerializeField] private AudioClip _roar;
    private bool _inCoroutine = false;
    [SerializeField, Range(1.0f, 15.0f)] private float _attackRate = 10.0f;
    private GameObject _mouseWeapon;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileParent;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _source = GetComponent<AudioSource>();
        _source.PlayOneShot(_roar);
    }
    
    void Update()
    {
        _rb.simulated = GameBehavior.Instance.State == Mousekeeping.GameState.Play;
        if (!_inCoroutine)
        {
            StartCoroutine(BossAttack());
            _mouseWeapon = Instantiate(_projectilePrefab, transform.position, Quaternion.identity, transform);
        }
    }

    void FixedUpdate()
    {
        RunBoss();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            BossManager.BossHealthDown = true;
            BossHealth--;
            Debug.Log(BossHealth);
            BossManager.BossHealthDown = false;
            if (BossHealth <= 0)
            {
                Destroy(gameObject);
                BossManager.BossHealthDown = false;
            }
        }
    }
    
    void RunBoss()
    {
        Vector2 direction = Random.onUnitCircle;
        _rb.AddForce(direction * _launchForce, ForceMode2D.Impulse);
        CheckSteepness(ref direction);
    }
    
    private void CheckSteepness(ref Vector2 direction)
    {
        if (Mathf.Abs(direction.x) < _steepnessThreshold)
        {
            direction.x += 0.5f * Mathf.Sign(direction.x);
            direction.Normalize();
        }
        else if (Mathf.Abs(direction.y) < _steepnessThreshold)
        {
            direction.y += 0.5f * Mathf.Sign(direction.y);
            direction.Normalize();
        }
    }
    
    IEnumerator BossAttack()
    {
        _inCoroutine = true;
        yield return new WaitForSeconds(_attackRate);
        _inCoroutine = false;
    }
}
