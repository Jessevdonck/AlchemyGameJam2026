using UnityEngine;

namespace ScriptableObjects.Inventory
{
    [CreateAssetMenu(menuName = "Game/Resource")]
    public class ResourceData : ScriptableObject
    {
        public string resourceName;
        public Sprite icon;
    }
}