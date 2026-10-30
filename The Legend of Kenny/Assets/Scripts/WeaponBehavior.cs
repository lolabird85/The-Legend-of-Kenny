using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    private AudioSource _source;
    [SerializeField] private AudioClip _squeak;
    [SerializeField, Range(5.0f, 50.0f)] float _projectileSpeed = 5.0f;

    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = _squeak;
    }
    
    void Update()
    {
        if (KenBehavior.FaceRight)
        {
            transform.Translate(_projectileSpeed * Time.deltaTime * Vector3.right);
        }
        else if (!KenBehavior.FaceRight)
        {
            transform.Translate(_projectileSpeed * Time.deltaTime * Vector3.left);
        }
        Destroy(gameObject, 0.18f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Mouse"))
        {
            _source.Play();
            Debug.Log(_source.clip.length);
        }
    }
}
