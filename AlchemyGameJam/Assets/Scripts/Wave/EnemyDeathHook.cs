using UnityEngine;

public class EnemyDeathHook : MonoBehaviour
{
    private EncounterDirector director;
    private bool isDead;

    public void Init(EncounterDirector dir)
    {
        director = dir;
        director.RegisterEnemy();
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        director.UnregisterEnemy();

        Destroy(gameObject);
    }
}