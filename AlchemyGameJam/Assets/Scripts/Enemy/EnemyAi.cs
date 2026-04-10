using UnityEngine;

public abstract class EnemyAi : BaseEnemy
{
    protected Transform player;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected abstract void TickAI();

    protected virtual void Update()
    {
        if (!player) return;

        TickAI();
    }
}