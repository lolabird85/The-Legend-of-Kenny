using Unity.VisualScripting;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _launchForce = 5.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float _steepnessThreshold = 0.25f;
    private Transform _crownPosition;
    public static int BossHealth = 1000;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        _rb.simulated = GameBehavior.Instance.State == Mousekeeping.GameState.Play;
    }

    void FixedUpdate()
    {
        RunBoss();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            BossHealth--;
            Debug.Log(BossHealth);
            if (BossHealth <= 0)
            {
                Destroy(gameObject);
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
}
