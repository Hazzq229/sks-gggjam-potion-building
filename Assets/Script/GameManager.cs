using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum Cause { Ingredient, Spell, Timeout}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    [Header("Game Settings")]
    [SerializeField] private float _maxOrderDuration;
    [SerializeField] private int _initialMoney;
    [SerializeField] private List<IsiPesananKustomer> _availableRecipes;
    [SerializeField] private int _rewardIngredient = 80;
    [SerializeField] private int _rewardTyping = 150;
    [SerializeField] private int _penaltyMisingredient = 50;
    [SerializeField] private int _penaltyMistype= 80;
    [SerializeField] private int _penaltyTimeout = 100;
    [Header("AudioClip")]
    [SerializeField] private AudioClip _angryCustomer;
    [SerializeField] private AudioClip _pleasedCustomer;
    [Header("Reference")]
    [SerializeField] private SpelledChoiceHandler _ingredientInteractionHandler;
    [SerializeField] private CustomerOrderUI _orderUI;
    [SerializeField] private TimeHandler _timeHandler;
    [SerializeField] private UangPemain _moneyHandler;
    [SerializeField] private AudioSource _soundFXHandler;
    [Header("Dynamic Timer Settings")]
    [Tooltip("Durasi minimal order")]
    [SerializeField] private float _minOrderDuration = 5f;
    [Tooltip("Pengurangan durasi setiap kenaikan tingkat kesulitan")]
    [SerializeField] private float _durationStep = 2f;
    [Tooltip("Turunkan durasi setiap berapa order selesai")]
    [SerializeField] private int _ordersPerStep = 3;
    [Tooltip("Tampilkan log perubahan durasi di Console")]
    [SerializeField] private bool _logDifficulty = true;
    [Header("Runtime")]
    [SerializeField] private IsiPesananKustomer _currentOrder;
    [SerializeField] private Image _currentCustomer;
    [SerializeField, Tooltip("Durasi order saat ini (runtime)")]
    private float _currentOrderDuration;
    [SerializeField, Tooltip("Jumlah order yang telah diselesaikan")]
    private int _ordersCompleted = 0;
    [Header("Event")]
    public UnityEvent OnTimeout;
    public UnityEvent OnCorrectIngredient;
    public UnityEvent OnMisIngredient;
    public UnityEvent OnTypingSuccess;
    public UnityEvent OnTypingFail;
    public UnityEvent OnAllIngredientsChosen;

    void Awake()
    {
        Instance = this;

        if (OnTimeout == null) OnTimeout = new UnityEvent();
        if (OnCorrectIngredient == null) OnCorrectIngredient = new UnityEvent();
        if (OnMisIngredient == null) OnMisIngredient = new UnityEvent();
        if (OnTypingSuccess == null) OnTypingSuccess = new UnityEvent();
        if (OnTypingFail == null) OnTypingFail = new UnityEvent();
        if (OnAllIngredientsChosen == null) OnAllIngredientsChosen = new UnityEvent();
    }

    void Start()
    {
        // Inisialisasi durasi saat ini dari nilai awal (_maxOrderDuration)
        _currentOrderDuration = Mathf.Max(_minOrderDuration, _maxOrderDuration);
        if (_timeHandler != null)
            _timeHandler.Countdown = _currentOrderDuration;
        _moneyHandler.MoneyCount = _initialMoney;

        OnCorrectIngredient.AddListener(() => HandleReward(Cause.Ingredient));
        OnTypingSuccess.AddListener(HandleCorrectSpell);
        OnMisIngredient.AddListener(() => HandlePenalty(Cause.Ingredient));
        OnTypingFail.AddListener(HandleMisSpell);
        OnTimeout.AddListener(HandleTimout);
        HandleOrderCreation();
    }
    public void HandleOrderCreation()
    {
        CreateOrder();
        RandomCustomerSprite();

        if (_timeHandler != null)
            _timeHandler.ResetTimer(_currentOrderDuration);
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


        _currentCustomer.sprite = _currentOrder.customersidle;

    }
    void customerreaction()
    {
       if (_moneyHandler.pleased_Cust == true) { 
        _currentCustomer.sprite = _currentOrder.customerpleased; }
        else if (_moneyHandler.Angry_Cust == true) { 
            _currentCustomer.sprite = _currentOrder.customerangry; }
        else
        {_currentCustomer.sprite = _currentOrder.customersidle;
            
        } 
    }
    void GameOver()
    {
        SceneManager.LoadScene("Gameover");
    }
    void Update()
    {

        customerreaction();
        if (_initialMoney <= 0)

        if (_moneyHandler.MoneyCount <= 0)

        {
            GameOver();
        }
    }
    void HandleCorrectSpell()
    {
        HandleReward(Cause.Spell);
        AdvanceDifficultyAfterOrder();
        HandleOrderCreation();
    }
    void HandleMisSpell()
    {
        HandlePenalty(Cause.Spell);
        AdvanceDifficultyAfterOrder();
        HandleOrderCreation();
    }
    void HandleTimout()
    {
        HandlePenalty(Cause.Timeout);
        AdvanceDifficultyAfterOrder();
        HandleOrderCreation();
    }
    void HandleReward(Cause cause)
    {
        switch (cause)
        {
            case Cause.Ingredient:
                _moneyHandler.AddMoney(_rewardIngredient);
                break;
            case Cause.Spell:
                _moneyHandler.AddMoney(_rewardTyping);
                break;
            default:
                break;
        }
        _soundFXHandler.clip = _pleasedCustomer;
        _soundFXHandler.Play();
    }
    void HandlePenalty(Cause cause)
    {
        switch (cause)
        {
            case Cause.Ingredient:
                _moneyHandler.DeductMoney(_penaltyMisingredient);
                break;
            case Cause.Spell:
                _moneyHandler.DeductMoney(_penaltyMistype);
                break;
            case Cause.Timeout:
                _moneyHandler.DeductMoney(_penaltyTimeout);
                break;    
        }
        _soundFXHandler.clip = _angryCustomer;
        _soundFXHandler.Play();
    }

    private void AdvanceDifficultyAfterOrder()
    {
        _ordersCompleted++;
        if (_ordersPerStep <= 0) return;

        if (_ordersCompleted % _ordersPerStep == 0)
        {
            float old = _currentOrderDuration;
            _currentOrderDuration = Mathf.Max(_minOrderDuration, _currentOrderDuration - _durationStep);
            if (_logDifficulty && !Mathf.Approximately(old, _currentOrderDuration))
            {
                Debug.Log($"[Difficulty] Orders={_ordersCompleted} -> orderDuration: {old:F1}s -> {_currentOrderDuration:F1}s (min={_minOrderDuration:F1})");
            }
        }
    }
}
