using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    public float attackRange = 2f;
    public float attackAngle = 60f;
    //public GameObject slashEffectPrefab;
    public float cooldown = 1f;
    public int damage = 50;
    public Animator anim;

 

    public override void Fire()
    {
            anim = gameObject.GetComponent<Animator>();
            anim.Play("Sword");
            //anim.SetTrigger("Fire");
    }
 
    public override void Upgrade()
    {
        base.Upgrade();
        attackRange *= 1.1f;
        attackAngle *= 1.1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage((int)damage);
        }
    }
}