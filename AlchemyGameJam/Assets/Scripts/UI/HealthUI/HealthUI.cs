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

        UpdateUI(playerHealth.CurrentHP);
    }

    private void UpdateUI(float currentHp)
    {
        Debug.Log("UI UPDATE: " + currentHp);
        healthSlider.maxValue = playerHealth.MaxHP;
        healthSlider.value = currentHp;

        healthText.text = $"{currentHp} / {playerHealth.MaxHP}";
    }

    private void OnDestroy()
    {
        playerHealth.OnHealthChanged -= UpdateUI;
    }
}