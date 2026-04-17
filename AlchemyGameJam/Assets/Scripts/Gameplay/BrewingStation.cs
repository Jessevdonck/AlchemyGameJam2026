using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class BrewingStation : Interactable
{
    
    public List<PotionBase> availablePotions;

    public float brewTime = 3f;

    public GameObject progressBar;
    public Slider progressSlider;

    public BrewingUI brewingUI;

    private bool _isBrewing;
    private bool _isReady;

    private PotionBase _brewedPotion;

    private Inventory _playerInventory;
    private InputReader _input;

    private void Awake()
    {
        _input = FindObjectOfType<InputReader>();
    }

    private void OnEnable()
    {
        _input.OnInteract += HandleInteract;
    }

    private void OnDisable()
    {
        _input.OnInteract -= HandleInteract;
    }

    private void HandleInteract()
    {
        Debug.Log("Interact check 1");
        if (_playerInventory == null) return;
        Debug.Log("Interact check 2");
        if (_isReady)
        {
            Debug.Log("Interact check 3");
            CollectPotion();
            return;
        }
        Debug.Log("Interact check 4");
        if (brewingUI.IsOpen)
        {
            brewingUI.Close();
            return;
        }
        Debug.Log("Interact check 5");
        brewingUI.Open(this, _playerInventory);
        Debug.Log("Interact check6 ");
    }

    public void StartBrewing(PotionBase potion)
    {
        if (_isBrewing) return;

        StartCoroutine(BrewRoutine(potion));
    }

    private IEnumerator BrewRoutine(PotionBase potion)
    {
        _brewedPotion = potion;

        _isBrewing = true;
        progressBar.SetActive(true);

        float t = 0;

        while (t < brewTime)
        {
            t += Time.deltaTime;
            progressSlider.value = t / brewTime;
            yield return null;
        }

        progressSlider.value = 1f;

        _isBrewing = false;
        _isReady = true;
    }

    private void CollectPotion()
    {
        if (_playerInventory.AddPotion(_brewedPotion))
        {
            _isReady = false;
            progressBar.SetActive(false);
            progressSlider.value = 0;
        }
    }

    protected override void ShowOutline()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _playerInventory = player.GetComponent<Inventory>();
    }

    protected override void HideOutline()
    {
        _playerInventory = null;
    }
}
