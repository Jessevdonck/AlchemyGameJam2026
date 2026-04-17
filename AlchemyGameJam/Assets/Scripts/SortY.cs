using UnityEngine;

public class YSort : MonoBehaviour
{
    private SpriteRenderer sr;
    
    private float lastY;

    [SerializeField] private float pivotOffset = 0f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        if (Mathf.Approximately(transform.position.y, lastY)) return;
        lastY = transform.position.y;
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
