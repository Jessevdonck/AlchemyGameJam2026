using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private InputReader input;
    private Camera cam;

    public void Initialize(InputReader inputReader)
    {
        input = inputReader;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    void Shoot()
    {
        // Vector2 worldPos = cam.ScreenToWorldPoint(input.MousePosition);
        // Vector2 dir = (worldPos - (Vector2)firePoint.position).normalized;
        //
        // GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        // projectile.GetComponent<Projectile>().Init(dir, projectileSpeed, gameObject);
    }
}