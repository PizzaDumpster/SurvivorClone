using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    public float speedMultiplier = 1.5f;
    public float duration = 5f;
    public float lifetime = 15f; // How long the buff exists before disappearing
    public float fadeDuration = 2f; // How long it takes to fade out
    public float floatAmplitude = 0.1f; // How much the buff floats up and down
    public float floatFrequency = 1f; // How fast the buff floats
    public Color buffColor = Color.cyan; // Unique color for speed buff

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
            Debug.LogError("SpriteRenderer not found on SpeedBuff. Please add a SpriteRenderer component.");
        }
        else
        {
            spriteRenderer.color = buffColor;
        }

        // Optional: Add a particle effect
        //AddParticleEffect();
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

        // Make the buff float up and down
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPosition + new Vector3(0, yOffset, 0);

        // Optional: Make the buff rotate
        //transform.Rotate(Vector3.forward, 50 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ApplySpeedBuff(speedMultiplier, duration);
                Destroy(gameObject);
            }
        }
    }

    //void AddParticleEffect()
    //{
    //    ParticleSystem particleSystem = GetComponent<ParticleSystem>();
    //    if (particleSystem == null)
    //    {
    //        particleSystem = gameObject.AddComponent<ParticleSystem>();
    //    }

    //    var main = particleSystem.main;
    //    main.startColor = buffColor;
    //    main.startSize = 0.1f;
    //    main.startSpeed = 1f;
    //    main.simulationSpace = ParticleSystemSimulationSpace.World;

    //    var emission = particleSystem.emission;
    //    emission.rateOverTime = 10;

    //    var shape = particleSystem.shape;
    //    shape.shapeType = ParticleSystemShapeType.Circle;
    //    shape.radius = 0.5f;

    //    particleSystem.Play();
    //}
}