using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    [Header("Game Settings")]
    [SerializeField] private float _orderDuration;
    [SerializeField] private int _initialMoney;
    [SerializeField] private List<IsiPesananKustomer> _availableRecipes;
    [SerializeField] private List<Sprite> _customerSprites;
    [Header("Reference")]
    [SerializeField] private SpelledChoiceHandler _ingredientInteractionHandler;
    [SerializeField] private CustomerOrderUI _orderUI;
    [SerializeField] private TimeHandler _timeHandler;
    // customer order ui manager
    [SerializeField] private UangPemain _moneyHandler;
    [Header("Runtime")]
    [SerializeField] private IsiPesananKustomer _currentOrder;
    [SerializeField] private Image _currentCustomer;

    void Start()
    {
        if (_timeHandler != null)
            _timeHandler.Countdown = _orderDuration;
        _moneyHandler.MoneyCount = _initialMoney;

        _ingredientInteractionHandler.OnOrderCompleted.AddListener(HandleOrderCreation);

        HandleOrderCreation();
    }
    public void HandleOrderCreation()
    {
        CreateOrder();
        RandomCustomerSprite();

        if (_timeHandler != null)
            _timeHandler.ResetTimer(_orderDuration);
        _ingredientInteractionHandler.StartNewOrder(_currentOrder);
        _orderUI.ShowOrderUI(_currentOrder);
    }
    void CreateOrder()
    {
        Debug.Log("Creating a new potion order.");
        int randomIndex = Random.Range(0, _availableRecipes.Count);
        _currentOrder = _availableRecipes[randomIndex];

        Debug.Log($"Current potion order: {_currentOrder.correctIngredient1}, {_currentOrder.correctIngredient2}, {_currentOrder.correctIngredient3}");
        Debug.Log($"Mantra: {_currentOrder.mantra}");
    }
    void RandomCustomerSprite()
    {
        Debug.Log("Changing customer sprite.");
        _currentCustomer.color = new Color(Random.value, Random.value, Random.value, 1f);

        // Use this if customer sprites are ready to be implemented
        // if (_customerSprites.Count > 0 && _currentCustomer != null)
        // {
        //     int randomSpriteIndex = Random.Range(0, _customerSprites.Count);
        //     _currentCustomer.sprite = _customerSprites[randomSpriteIndex];
        // }
    }
}
