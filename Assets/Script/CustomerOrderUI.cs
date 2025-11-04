using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrderUI : MonoBehaviour
{
    [Header("UI Slots")]
    public Image slotBahan1;
    public Image slotBahan2;
    public Image slotBahan3;

    [System.Serializable]
    public class SpriteBahan1 {
        public Bahan1 type;
        public Sprite icon;
    }
    [System.Serializable]
    public class SpriteBahan2 {
        public Bahan2 type;
        public Sprite icon;
    }
    [System.Serializable]
    public class SpriteBahan3 {
        public Bahan3 type;
        public Sprite icon;
    }
    [Header("Sprite Database")]
    public List<SpriteBahan1> dbBahan1;
    public List<SpriteBahan2> dbBahan2;
    public List<SpriteBahan3> dbBahan3;

    // Fungsi 'Find' sederhana untuk database
    private Sprite GetSprite(Bahan1 type) => dbBahan1.Find(item => item.type == type)?.icon;
    private Sprite GetSprite(Bahan2 type) => dbBahan2.Find(item => item.type == type)?.icon;
    private Sprite GetSprite(Bahan3 type) => dbBahan3.Find(item => item.type == type)?.icon;
    public void ShowOrderUI(IsiPesananKustomer order)
    {
        slotBahan1.sprite = GetSprite(order.correctIngredient1); 
        slotBahan2.sprite = GetSprite(order.correctIngredient2);
        slotBahan3.sprite = GetSprite(order.correctIngredient3);

        slotBahan1.color = (slotBahan1.sprite == null) ? Color.clear : Color.white;
        slotBahan2.color = (slotBahan2.sprite == null) ? Color.clear : Color.white;
        slotBahan3.color = (slotBahan3.sprite == null) ? Color.clear : Color.white;
    }
    public void HideOrderUI()
    {
        slotBahan1.color = Color.clear;
        slotBahan2.color = Color.clear;
        slotBahan3.color = Color.clear;
    }
}