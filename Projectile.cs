// Projectile.cs (updated)
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;
    public int damageAmount = 25;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!MapBoundary.Instance.ClampPosition(transform.position).Equals((Vector2)transform.position))
        {
            Destroy(gameObject);
        }
    }
}