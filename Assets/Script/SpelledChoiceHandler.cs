using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpelledChoiceHandler : MonoBehaviour
{
    private IsiPesananKustomer pesanan;
    private UangPemain duid;
    private int bahanindex;
    public Button[] tombolbahan1;
    public Button[] tombolbahan2;
    public Button[] tombolbahan3;
    
    public InputField ketikmantra;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void milihbahan(int indexbahan )
    {
        if (bahanindex == pesanan.bahanbenar1)
        {
            duid.tambahuang(100f);
        }
        else
        {
            duid.kuranguang(50f);
        }
        bahanindex++;
    }
    
}
