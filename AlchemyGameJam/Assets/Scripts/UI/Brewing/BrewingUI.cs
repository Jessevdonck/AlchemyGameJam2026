using System;
using UnityEngine;
using Player;
using ScriptableObjects.Inventory;

public class BrewingUI : MonoBehaviour
{
    public GameObject root;
    public Transform cardParent;
    public PotionCardUI cardPrefab;

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

        ClearCards();

        root.SetActive(true);
        IsOpen = true;

        GenerateCards();
    }

    private void ClearCards()
    {
        foreach (Transform child in cardParent)
            Destroy(child.gameObject);
    }

    public void Close()
    {
        root.SetActive(false);
        IsOpen = false;
        ClearCards();
    }

    private void GenerateCards()
    {
        foreach (var potion in _station.availablePotions)
        {
            var card = Instantiate(cardPrefab, cardParent);
            card.Setup(potion, TryBrew);
        }
    }

    private void TryBrew(PotionBase potion)
    {
        if (!_inventory.TrySpendResource(potion.costResource, potion.costAmount))
        {
            Debug.Log("Not enough resource");
            return;
        }

        _station.StartBrewing(potion);
        Close();
    }
}