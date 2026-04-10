using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerShooting shooting;

    private InputReader input;

    private void Awake()
    {
        input = FindAnyObjectByType<InputReader>();

        movement.Initialize(input);
        shooting.Initialize(input);
    }
}