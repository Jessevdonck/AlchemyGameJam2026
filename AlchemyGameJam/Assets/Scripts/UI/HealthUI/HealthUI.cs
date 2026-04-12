using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        playerHealth.OnHealthChanged += UpdateUI;

        UpdateUI(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }

    private void UpdateUI(float current, float max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;

        healthText.text = $"{current} / {max}";
    }

    private void OnDestroy()
    {
        playerHealth.OnHealthChanged -= UpdateUI;
    }
}