
using UnityEngine;

public class MapBoundary : MonoBehaviour
{
    public static MapBoundary Instance { get; private set; }

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector2 ClampPosition(Vector2 position)
    {
        float x = Mathf.Clamp(position.x, minX, maxX);
        float y = Mathf.Clamp(position.y, minY, maxY);
        return new Vector2(x, y);
    }
}