using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField, Range(5.0f, 50.0f)] float _projectileSpeed = 10.0f;
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
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
