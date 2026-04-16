using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;

    // The SpriteRenderer on this GameObject (or a child) to outline
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Tweak these in the Inspector
    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] private float outlineSize = 2f;

    private Material _material;
    private static readonly int OutlineEnabled = Shader.PropertyToID("_OutlineEnabled");
    private static readonly int OutlineColor   = Shader.PropertyToID("_OutlineColor");
    private static readonly int OutlineSizeProp = Shader.PropertyToID("_OutlineSize");

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        _material = new Material(Shader.Find("Custom/SpriteOutline"));
        spriteRenderer.material = _material;

        _material.SetFloat(OutlineEnabled, 0f);
        _material.SetColor(OutlineColor, outlineColor);
        _material.SetFloat(OutlineSizeProp, outlineSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        ShowOutline();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        HideOutline();
    }

    private void ShowOutline()
    {
        _material.SetFloat(OutlineEnabled, 1f);
    }

    private void HideOutline()
    {
        _material.SetFloat(OutlineEnabled, 0f);
    }

    private void OnDestroy()
    {
        // Clean up the instance material
        if (_material != null)
            Destroy(_material);
    }
}