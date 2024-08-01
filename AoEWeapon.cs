// AoEWeapon.cs (updated)
using UnityEngine;

public class AoEWeapon : WeaponBase
{
    public float radius = 3f;
    public GameObject visualEffect;
    public float cooldown = 1;
    public int damage = 25; 

    public override void Fire()
    {
        if (Time.time >= lastFireTime + cooldown)
        {
            lastFireTime = Time.time;

            if (visualEffect != null)
            {
                GameObject effect = Instantiate(visualEffect, transform.position, Quaternion.identity);
                effect.transform.localScale = Vector3.one * radius;
                Destroy(effect, 0.5f);
            }

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    Enemy enemy = hitCollider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage((int)damage);
                    }
                }
            }
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();
        radius *= 1.1f;
    }
}