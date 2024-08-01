// Weapon.cs
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;

    private float nextFireTime;

    void Update()
    {
        Vector2 aimDirection = InputManager.Instance.GetAimDirection();

        if (aimDirection.magnitude > 0.1f && Time.time >= nextFireTime)
        {
            Fire(aimDirection);
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Fire(Vector2 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * projectileSpeed;
    }
}