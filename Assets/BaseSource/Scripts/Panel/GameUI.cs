using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameUI : Panel
{
    public static GameUI Instance;
    [SerializeField]
    private TextMeshProUGUI roundText;
    [SerializeField]
    private AudioClip retrySFX, homeSFX;
    public HealthBar healthBar;
    public BossWarning roundSign;

    public PowerBar powerBar;
    public override void PostInit()
    {
        Instance = this;
    }

    public override void Close()
    {
        Deactive();
    }
    public override void Deactive()
    {
        SoundManager.Instance.PlaySFX(SFX.popUpClose, 0.5f);
        BackButtonController.Instance.UnRegister(this);
        base.Deactive();
    }
    public void ToggleGrid()
    {

        HighLightMap.Instance.Toggle();
    }
    public void OpenPause()
    {
        Close();
        GameController.Instance.Close();

    }
    public void SetUp(int level, GameMode mode)
    {
        canBack = true;
        Show();
        roundSign.Deactive();


    }
    public void SetRound(int round)
    {
        roundText.text = "Round " + round;
    }
    public void Retry()
    {
        GameController.Instance.Retry();
        SoundManager.Instance.PlaySFX(retrySFX, 1);
    }
    public void Back()
    {

    }



    public void ShowResult(int level, int earnedCoin, int earnedGem)
    {
        GameUI.Instance.canBack = false;
        if (SettingPanel.Instance.gameObject.activeSelf)
        {
            SettingPanel.Instance.Close();
        }
        StartCoroutine(DoShowResult(level, earnedCoin, earnedGem));
    }
    IEnumerator DoShowResult(int level, int earnedCoin, int earnedGem)
    {
        yield return new WaitForSeconds(1.5f);

        SoundManager.Instance.PlaySFX(SFX.clap, 1f);

        EffectManager.Instance.PlayFireWork2();


        Deactive();

        ResultPanel.Instance.SetUp(level, earnedCoin, earnedGem);
        LevelMap map = GameController.Instance.levelManager.selectedLevelMap;
        map.Deactive();

    }

    public void ShowResultFail(int level, int earnedCoin)
    {
        GameUI.Instance.canBack = false;

        StartCoroutine(DoShowResultFail(level, earnedCoin));
    }

    IEnumerator DoShowResultFail(int level, int earnedCoin)
    {
        GameController.Instance.currentEarnedCoin = 0;
        GameController.Instance.currentEarnedGem = 0;
        yield return new WaitForSeconds(1f);
        ResultPanelFail.Instance.SetUp(level);

        LevelMap map = GameController.Instance.levelManager.selectedLevelMap;
        map.Deactive();
        Deactive();


    }
  
   
    public bool canBack = false;
    public void ToHome()
    {
        GameController.Instance.Close();
    }
    public override void Hide()
    {
        ani.SetTrigger("Close");
    }
    public override void Active()
    {
        gameObject.SetActive(true);
    }

    public override void Show()
    {
        Active();
    }


}
