using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class UangPemain : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    private int _moneyCount;
      public int MoneyCount
     {
        get
        {
            return _moneyCount;
        }
        set
        {
            _moneyCount = value;

            _moneyText.text = ""+_moneyCount;
            PlayerPrefs.SetInt("Uang", _moneyCount);
        }
    }
    void Start()
    {
        _moneyCount = PlayerPrefs.GetInt("Uang", 0);
        AddMoney(1000);
    }
    public void AddMoney(int amount)
    {
        Debug.Log("Adding money to player's balance.");
        Debug.Log("Customer Pleased");
        MoneyCount += amount;
    }
    public void DeductMoney(int amount)
    {
        Debug.Log("Customer Angry");
        Debug.Log("Deducting money from player's balance.");
        MoneyCount -= amount;
    }
}
