using UnityEngine;
using UnityEngine.UI;

public class SpelledChoiceHandler : MonoBehaviour
{
    public IsiPesananKustomer pesanan;
    public TimeHandler waktu;
    public UangPemain duid;

    public GameObject[] kelompokBahan;

    private int bahanindex = 0;

    void Start()
    {

        for (int i = 0; i < kelompokBahan.Length; i++)
            kelompokBahan[i].SetActive(i == 0);


        for (int group = 0; group < kelompokBahan.Length; group++)
        {
            Button[] buttons = kelompokBahan[group].GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                int capturedGroup = group;
                int capturedIndex = i;
                buttons[i].onClick.AddListener(() => PilihBahan(capturedGroup, capturedIndex));
            }
        }
        waktu.TimeUpEvent += HandleTimeUp;
    }

    void PilihBahan(int group, int index)
    {

        if (group != bahanindex)
            return;

        int correctIndex = -1;

        switch (bahanindex)
        {
            case 0: correctIndex = pesanan.bahanbenar1; break;
            case 1: correctIndex = pesanan.bahanbenar2; break;
            case 2: correctIndex = pesanan.bahanbenar3; break;
        }

        if (waktu.countdown > 0)
        {
            if (index == correctIndex)
            {
                duid.tambahuang(100f);

            }
            else
            {
                duid.kuranguang(50f);

            }




            kelompokBahan[bahanindex].SetActive(false);

            bahanindex++;
        }
        else if (waktu.countdown <= 0)
        {
            Debug.Log("Time up");
            waktu.countdown += 10f;
            duid.kuranguang(50f);
            kelompokBahan[bahanindex].SetActive(false);

            bahanindex++;
        }

        if (bahanindex < kelompokBahan.Length)
        {
            kelompokBahan[bahanindex].SetActive(true);
        }
        
    


    }
    void HandleTimeUp()
{
    Debug.Log("Time's up! Penalize player.");
    duid.kuranguang(50f);
    waktu.countdown = waktu.timeslide.maxValue;
    kelompokBahan[bahanindex].SetActive(false);
    bahanindex++;
    if (bahanindex < kelompokBahan.Length)
        kelompokBahan[bahanindex].SetActive(true);
}
}
