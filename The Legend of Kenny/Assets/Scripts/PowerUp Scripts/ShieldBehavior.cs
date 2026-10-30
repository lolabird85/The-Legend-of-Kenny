using System;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldBehavior : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("projectile"))
        {
            Debug.Log("collision with projectile");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("projectile"))
        {
            Debug.Log("trigger with projectile");
            Destroy(other.gameObject);
        }
    }
}
