using Gameplay;
using UnityEngine;

public class RitualStart : Interactable
{
    public void Interact()
    {
        RitualSystem.Instance.TriggerRitual();
    }
}
