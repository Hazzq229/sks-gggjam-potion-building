using System;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpelledChoiceHandler : MonoBehaviour
{
    [Header("Crafting Settings")]
    [SerializeField] private GameObject[] _ingredientsGroup;
    [Header("Reference")]
    [SerializeField] private TimeHandler _timeHandler;
    [SerializeField] private UangPemain _moneyHandler;
    [SerializeField] private AudioSource _soundeffecthandler;
    [Header("AudioClip")]
    [SerializeField] private AudioClip _angrycustomer;
    [SerializeField] private AudioClip _pleasedcustomer;
    
    [Header("Runtime")]
    [SerializeField] private IsiPesananKustomer _currentOrder;
    [Header("Event")]
    public UnityEvent OnOrderCompleted;
    private int _ingredients = 0;

    void Awake()
    {
        for (int i = 0; i < _ingredientsGroup.Length; i++)
            _ingredientsGroup[i].SetActive(false);
    }
    void Start()
    {
        // Add listener for each button 
        for (int group = 0; group < _ingredientsGroup.Length; group++)
        {
            Button[] buttons = _ingredientsGroup[group].GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                int capturedGroup = group;
                int capturedIndex = i;
                buttons[i].onClick.AddListener(() => ChooseIngredient(capturedGroup, capturedIndex));
            }
        }
        _timeHandler.TimeUpEvent += HandleTimeUp;
    }
    public void StartNewOrder(IsiPesananKustomer newOrder)
    {
        Debug.Log("Starting a new potion order");
        _currentOrder = newOrder;
        _ingredients = 0;

        if (_ingredientsGroup.Length > 0)
            _ingredientsGroup[0].SetActive(true);
    }
    void ChooseIngredient(int group, int index)
    {
        if (_currentOrder == null || group != _ingredients) return;
        bool isCorrect = false;
        switch (_ingredients)
        {
            case 0:
                isCorrect = ((Bahan1)index) == _currentOrder.correctIngredient1;
                break;
            case 1:
                isCorrect = ((Bahan2)index) == _currentOrder.correctIngredient2;
                break;
            case 2:
                isCorrect = ((Bahan3)index) == _currentOrder.correctIngredient3;
                break;
        }

        if (isCorrect)
        {
            Debug.Log("Correct potion material.");
            _moneyHandler.AddMoney(100);
            _soundeffecthandler.clip = _pleasedcustomer;
            _soundeffecthandler.Play();
        }
        else
        {
            Debug.Log("Incorrect potion material.");
            _moneyHandler.DeductMoney(50);
            _soundeffecthandler.clip = _angrycustomer;
            _soundeffecthandler.Play();
        }

        ContinueNextMaterial();
    }
    void HandleTimeUp()
    {
        Debug.Log("Time's up! Penalize player.");
        _moneyHandler.DeductMoney(100);

        OnOrderCompleted.Invoke();
    }
    void ContinueNextMaterial()
    {
        _ingredientsGroup[_ingredients].SetActive(false);

        _ingredients++;

        if (_ingredients >= _ingredientsGroup.Length)
        {
            Debug.Log("Order Completed");
            OnOrderCompleted.Invoke();
        }
        else
        {
            _ingredientsGroup[_ingredients].SetActive(true);
        }
    }
}
