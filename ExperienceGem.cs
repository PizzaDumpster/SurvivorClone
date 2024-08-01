using UnityEngine;

public class ExperienceGem : MonoBehaviour
{
    public static float experienceMultiplier = 2f;
    public int baseExperienceValue = 25;
    public int experienceValue = 25;
    public float magnetSpeed = 5f;
    public float magnetDistance = 5f;
    public float lifetime = 10f; // How long the gem exists before disappearing
    public float fadeDuration = 2f; // How long it takes to fade out

    private Transform player;
    private float creationTime;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        creationTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on ExperienceGem. Please add a SpriteRenderer component.");
        }
    }

    void Update()
    {
        float elapsedTime = Time.time - creationTime;

        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
            return;
        }

        // Start fading out when approaching expiration
        if (elapsedTime >= lifetime - fadeDuration)
        {
            float fadeProgress = (elapsedTime - (lifetime - fadeDuration)) / fadeDuration;
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 1 - fadeProgress;
                spriteRenderer.color = color;
            }
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= magnetDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, magnetSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                int experienceValue = Mathf.RoundToInt(baseExperienceValue * experienceMultiplier);
                playerController.CollectExperience(experienceValue);
                Destroy(gameObject);
            }
        }
    }
    public static void UpgradeExperienceValue()
    {
        experienceMultiplier *= 1.2f; // Increase by 20% each upgrade
    }
}