using Enum;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;

    public Team team = Team.Player;
    private InputReader input;

    private void Awake()
    {
        input = FindAnyObjectByType<InputReader>();

        movement.Initialize(input);
    }
}