using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UangPemain : MonoBehaviour
{
    public float uang; 
    // Start is called before the first frame update
    void Start()
    {
      uang =  PlayerPrefs.GetFloat("Uang", 0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
