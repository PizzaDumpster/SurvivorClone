using Unity.VisualScripting;
using UnityEngine;

public class ProjectileWeapon : WeaponBase
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float detectionRadius = 30f; // Radius to search for enemies
    public float cooldown = 1; 

    public override void Fire()
    {
        if (Time.time >= lastFireTime + cooldown)
        {
            Transform closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                Vector2 direction = (closestEnemy.position - transform.position).normalized;
                rb.velocity = direction * projectileSpeed;
                lastFireTime = Time.time;
            }
        }
    }

    private Transform FindClosestEnemy()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider.transform;
                }
            }
        }

        return closestEnemy;
    }

    protected virtual void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position;
        }
    }
}