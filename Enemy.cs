using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int damageAmount = 10;
    public float attackCooldown = 1f;
    public int maxHealth = 100;
    public int currentHealth;
    public Slider enemyHealthBar;
    public GameObject experienceGemPrefab;
    public int experienceValue = 1;

    public float dropRadius = 1f; // Radius around the enemy to check for drop positions
    public int maxDropAttempts = 10;

    [System.Serializable]
    public class DropChance
    {
        public GameObject itemPrefab;
        [Range(0f, 1f)]
        public float chance;
    }

    public DropChance[] dropChances;

    private Transform player;
    private PlayerHealth playerHealth;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealthBar = GetComponentInChildren<Slider>();
        currentHealth = maxHealth;
        enemyHealthBar.value = currentHealth;
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= lastAttackTime + attackCooldown)
        {
            playerHealth.TakeDamage(damageAmount);
            lastAttackTime = Time.time;
        }
    }

    public void TakeDamage(int damage)
    {       
        currentHealth -= damage;
        enemyHealthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            
            Die();
        }
    }

    void Die()
    {
        // You can add effects, sound, or item drops here
        DropItems();
        Destroy(gameObject);
    }

    void DropItems()
    {
        foreach (DropChance dropChance in dropChances)
        {
            if (Random.value <= dropChance.chance)
            {
                Vector2 dropPosition = FindFreeDropPosition();
                Instantiate(dropChance.itemPrefab, dropPosition, Quaternion.identity);
            }
        }

        // Always drop experience gem
        Vector2 gemPosition = FindFreeDropPosition();
        Instantiate(experienceGemPrefab, gemPosition, Quaternion.identity);
    }
    Vector2 FindFreeDropPosition()
    {
        for (int i = 0; i < maxDropAttempts; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * dropRadius;
            Vector2 potentialPosition = (Vector2)transform.position + randomOffset;

            if (IsPositionFree(potentialPosition))
            {
                return potentialPosition;
            }
        }

        // If no free position found, return the enemy's position
        return transform.position;
    }
    bool IsPositionFree(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("ExperienceGem") || collider.CompareTag("Item"))
            {
                return false;
            }
        }
        return true;
    }

}