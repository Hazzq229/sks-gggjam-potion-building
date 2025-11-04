using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UangPemain : MonoBehaviour
{
    public float uang;
    public TextMeshProUGUI jumlahduid; 
    // Start is called before the first frame update
    void Start()
    {
      uang =  PlayerPrefs.GetFloat("Uang", 0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        jumlahduid.text = "Uangmu  " + uang;
    }
    public void tambahuang(float jumlah)
    {
        uang += jumlah;
    }
    public void kuranguang(float jumlah)
    {
        uang -= jumlah;
    }
}
