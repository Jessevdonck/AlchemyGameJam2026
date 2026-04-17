using Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        playerHealth.OnHealthChanged += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        healthSlider.maxValue = playerHealth.MaxHP;
        healthSlider.value = playerHealth.CurrentHP;

        healthText.text = $"{playerHealth.CurrentHP} / {playerHealth.MaxHP}";
    }

    private void OnDestroy()
    {
        playerHealth.OnHealthChanged -= UpdateUI;
    }
}