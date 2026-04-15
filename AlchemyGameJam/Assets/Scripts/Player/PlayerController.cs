using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;

    private InputReader input;

    private void Awake()
    {
        input = FindAnyObjectByType<InputReader>();

        movement.Initialize(input);
    }
}