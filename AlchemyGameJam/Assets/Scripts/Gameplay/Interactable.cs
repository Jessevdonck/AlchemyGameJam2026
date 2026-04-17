using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    private bool isInteractable = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        isInteractable = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        isInteractable = false;
    }
    
    protected  void ShowOutline()
    {
        
    }

    protected  void HideOutline()
    {
        
    }
    

    private void OnDestroy()
    {
    }
}