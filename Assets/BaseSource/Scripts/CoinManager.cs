using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public int currentCoin = 1000,currentGem;
    [SerializeField]
    private CoinPanel[] coinPanels,gemPanels;
    public delegate void OnUpdate(string coin,string gem);
    public static OnUpdate onUpdate;

    public delegate void OnAdded(int amount);
    public static OnAdded onAdded;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        currentCoin = PrefInfo.GetCoin(); 
        currentGem = PrefInfo.GetGem();
        UpdateText();
    }
    
    public int GetCoin()
    {
        return currentCoin;
    }
    public void AddCoin(int amount)
    {
        //if (amount < 0)
        //{
        //    PrefInfo.SetAchivementProgress(4, PrefInfo.GetAchivementProgress(4) -amount);

        //}
        onAdded?.Invoke(amount);
        UpdateCoin(GetCoin() + amount);
    }
    public void UpdateCoin(int coin)
    {
        currentCoin = coin;
         

        UpdateText();
        PrefInfo.SetCoin(currentCoin);
    }
    public int GetGem()
    {
        return currentGem;
    }
    public void AddGem(int amount)
    {
        UpdateGem(GetGem() + amount);
    }
    public void UpdateGem(int gem)
    {
        currentGem = gem;
        UpdateText();
        PrefInfo.SetGem(currentGem);
    }
    
    public void UpdateText()
    {
        string coin = currentCoin.ToString();
        string gem = currentGem.ToString();
        onUpdate?.Invoke(coin,gem);
        //for (int i = 0; i < coinPanels.Length; i++)
        //{
        //    coinPanels[i].UpdateCoin(coin);
        //}

        ////
        //string gem = currentGem.ToString();

        //for (int i = 0; i < gemPanels.Length; i++)
        //{
        //    gemPanels[i].UpdateCoin(gem);
        //}
    }

    public void Show(int index = -1)
    {
        //if (index == -1)
        //{
        //    //Debug.Log("SHOW1");
        //    for (int i = 0; i < coinPanels.Length; i++)
        //    {
        //        //Debug.Log("SHOW "+i );

        //        coinPanels[i].Show();
        //    }
        //}
        //else
        //{
        //    //Debug.Log("SHOW2");
        //    Hide();
        //    coinPanels[index].Show();
        //}
    }
    public void Hide(int index = -1)
    {
        ////Debug.Log("HIDE");
        //if (index == -1)
        //{
        //    for (int i = 0; i < coinPanels.Length; i++)
        //    {
        //        coinPanels[i].Hide();
        //    }
        //}
        //else
        //{
        //    coinPanels[index].Hide();
        //}
    }

    public void ShowGem(int index = -1)
    {
        //if (index == -1)
        //{
        //    for (int i = 0; i < gemPanels.Length; i++)
        //    {
        //        gemPanels[i].Show();
        //    }
        //}
        //else
        //{
        //    //Debug.Log("SHOW2");
        //    HideGem();
        //    gemPanels[index].Show();
        //}
    }
    public void HideGem(int index = -1)
    {
        ////Debug.Log("HIDE");
        //if (index == -1)
        //{
        //    for (int i = 0; i < gemPanels.Length; i++)
        //    {
        //        gemPanels[i].Hide();
        //    }
        //}
        //else
        //{
        //    gemPanels[index].Hide();
        //}
    }
}
