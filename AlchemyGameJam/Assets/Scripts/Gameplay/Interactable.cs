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
    private static readonly int OutlineEnabled = Shader.PropertyToID("_OutlineState");
    private static readonly int OutlineColor   = Shader.PropertyToID("_Outline_Color");

    private void Awake()
    {
        _material = gameObject.GetComponent<Renderer>().material;
        spriteRenderer.material = _material;

        _material.SetFloat(OutlineEnabled, 0f);
        _material.SetColor(OutlineColor, outlineColor);
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
        _material.SetFloat(OutlineEnabled, 0.03125f);
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