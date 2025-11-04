using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewCustomerOrder", menuName = "Create Customer Order")]

public class IsiPesananKustomer : ScriptableObject
{
    public Bahan1 correctIngredient1;
    public Bahan2 correctIngredient2;
    public Bahan3 correctIngredient3;
    public string mantra;
}
