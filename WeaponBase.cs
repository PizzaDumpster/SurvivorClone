using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    public float baseDamage;
    public float baseCooldown;

    protected float currentDamage;
    protected float currentCooldown;
    protected float lastFireTime;

    protected Transform playerTransform;

    public virtual void Initialize(Transform player)
    {
        playerTransform = player;
        transform.localPosition = Vector3.zero;
        ResetStats();
    }

    public virtual void ResetStats()
    {
        currentDamage = baseDamage;
        currentCooldown = baseCooldown;
        lastFireTime = 0f;
    }

    public abstract void Fire();

    public virtual void Upgrade()
    {
        currentDamage *= 1.2f;
        currentCooldown *= 0.9f;
    }

    
}