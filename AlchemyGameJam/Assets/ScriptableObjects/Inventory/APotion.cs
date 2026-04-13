using UnityEngine;

namespace ScriptableObjects.Inventory
{
    public abstract class APotion : ScriptableObject
    {
        public string potionName;
        public Sprite icon;

        public abstract void Use(GameObject user);
    }
}
