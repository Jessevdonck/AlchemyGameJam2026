using TMPro;
using UnityEngine;

public class RoundUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundText;

    private void Start()
    {
        var director = FindObjectOfType<EncounterDirector>();
        director.OnRoundChanged += SetRound;
    }

    public void SetRound(int round)
    {
        roundText.text = $"{round}";
    }
}