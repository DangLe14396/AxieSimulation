using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;
public class ResultPanelFail : Panel
{
    public static ResultPanelFail Instance;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Button adBtn;
    [SerializeField]
    private GameObject  skipBtn, retryBtn;
   
    bool canClick = false;
    public override void PostInit()
    {
        Instance = this;
    }
    public void SetUp(int level)
    {
        canClick = true;
        skipBtn.SetActive(false);
        levelText.text = "LEVEL " + ((level < 10 ? "00" : (level < 100 ? "0" : "")) + level).ToString(); ;
        retryBtn.SetActive(true);

        skipBtn.SetActive(true);
        Active();


    }
   
    public void Home()
    {
        Close();
        GameController.Instance.Close();
    }
    public override void Close()
    {
        base.Close();
    }
    public void Retry()
    {
        if (!canClick) return;
        canClick = false;
        Close();
        GameController.Instance.Retry();
    }
    bool canClickAd = true;

   
    public override void OnBack()
    {
         Retry();
    }

}
