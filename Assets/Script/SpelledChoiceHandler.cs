using System;
using TMPro;
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
    [Header("Runtime")]
    [SerializeField] private IsiPesananKustomer _currentOrder;
    [SerializeField] private string _mantraWord;
    private int _ingredients = 0;

    public IsiPesananKustomer CurrentOrder => _currentOrder;

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
            GameManager.Instance.OnCorrectIngredient.Invoke();
        }
        else
        {
            Debug.Log("Incorrect potion material.");
            GameManager.Instance.OnMisIngredient.Invoke();
        }

        ContinueNextMaterial();
    }
    void HandleTimeUp()
    {
        Debug.Log("Time's up! Penalize player.");

        GameManager.Instance.OnTimeout.Invoke();
    }
    void ContinueNextMaterial()
    {
        _ingredientsGroup[_ingredients].SetActive(false);

        _ingredients++;

        if (_ingredients >= _ingredientsGroup.Length)
        {
            Debug.Log("All Ingredient Has Been Add to Cauldron");
            GameManager.Instance.OnAllIngredientsChosen.Invoke();
        }
        else
        {
            _ingredientsGroup[_ingredients].SetActive(true);
        }
    }
}
