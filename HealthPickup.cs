using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 20;
    public float lifetime = 15f; // How long the pickup exists before disappearing
    public float fadeDuration = 2f; // How long it takes to fade out
    public float floatAmplitude = 0.1f; // How much the pickup floats up and down
    public float floatFrequency = 1f; // How fast the pickup floats

    private float creationTime;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;

    void Start()
    {
        creationTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on HealthPickup. Please add a SpriteRenderer component.");
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

        // Make the pickup float up and down
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPosition + new Vector3(0, yOffset, 0);

        // Optional: Make the pickup pulse
        float scale = 1 + Mathf.Sin(Time.time * floatFrequency * 2) * 0.1f;
        transform.localScale = Vector3.one * scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}