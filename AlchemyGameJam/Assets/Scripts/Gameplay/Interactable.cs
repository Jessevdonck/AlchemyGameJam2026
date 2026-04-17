using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;

    

    
    private void Awake()
    {
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
    }

    private void HideOutline()  
    {
    }

    private void OnDestroy()
    {
      
    }
}