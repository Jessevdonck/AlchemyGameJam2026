using ScriptableObjects.Inventory;
using UnityEngine;

[CreateAssetMenu(menuName = "Potions/Potion")]
public class PotionBase : ScriptableObject
{
    public string potionName;
    public Sprite icon;

    [Header("Cost")]
    public ResourceData sulfurCost;
    public int sulfurAmount = 1;

    [Header("Effect")]
    public PassiveEffectBase passivePrefab;

    public void Use(GameObject target)
    {
        if (passivePrefab == null) return;

        var instance = Instantiate(passivePrefab, target.transform);
        instance.OnActivate();
    }
}