using System;
using UnityEngine;
using UnityEngine.UI;
using Player;

public class BrewingUI : MonoBehaviour
{
    public GameObject root;
    public Transform buttonParent;
    public Button buttonPrefab;

    public bool IsOpen;
    private BrewingStation _station;
    private Inventory _inventory;

    private void Awake()
    {
        root.SetActive(false);
        IsOpen = false;
    }

    public void Open(BrewingStation station, Inventory inventory)
    {
        _station = station;
        _inventory = inventory;

        ClearButtons(); 

        root.SetActive(true);
        IsOpen = true;
        GenerateButtons();
    }

    private void ClearButtons()
    {
        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);
    }

    public void Close()
    {
        root.SetActive(false);
        IsOpen = false;
        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);
    }

    private void GenerateButtons()
    {
        foreach (var potion in _station.availablePotions)
        {
            var btn = Instantiate(buttonPrefab, buttonParent);

            var text = btn.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            text.text = $"{potion.potionName} ({potion.sulfurAmount})";

            btn.onClick.AddListener(() => TryBrew(potion));
        }
    }

    private void TryBrew(PotionBase potion)
    {
        if (!_inventory.TrySpendResource(potion.sulfurCost, potion.sulfurAmount))
        {
            Debug.Log("Not enough sulfur");
            return;
        }

        _station.StartBrewing(potion);
        Close();
    }
}