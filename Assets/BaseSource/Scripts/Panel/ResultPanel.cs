using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultPanel : Panel
{
    public static ResultPanel Instance;
    public bool canClick = false;
    [SerializeField]
    Spine.Unity.SkeletonGraphic axieAnim;
    [SerializeField]
    private Spine.Unity.SkeletonDataAsset[] assets;

    [SerializeField]
    private AudioClip nextSFX;
    public override void PostInit()
    {
        Instance = this;
    }

    public override void Show()
    {
        base.Show();
    }
    int activeCount = 0;
    public void SetUp(int level, long earnedCoin,long earnedGem)
    {

        level = GameController.Instance.currentLevel;
        activeCount++;
        Active();
    
      
        canClick = true;

        axieAnim.skeletonDataAsset = assets[GameController.Instance.victoryTeam];
        axieAnim.startingLoop = true;
        axieAnim.startingAnimation = "action/victory-pose-back-flip";
        axieAnim.Initialize(true);


        SoundManager.Instance.PlaySFX(SFX.firework, 0.5f);
        EffectManager.Instance.PlayFireWork2();

    }


    public void ToHome()
    {
        if (!canClick) return;
        canClick = false;
        Close();
        GameController.Instance.Close();
       
    }
    public override void Close()
    {
        if (!BackButtonController.Instance.canBack)
        {
            BackButtonController.Instance.canBack = true;
        }
        base.Close();
    }
    public void Next()
    {
        if (!canClick) return;
        canClick = false;
        SoundManager.Instance.PlaySFX(nextSFX, 1);
        Close();
        GameController.Instance.NextLevel();
      

    }
   
  
    public override void OnBack()
    {
    }

    public void Retry()
    {
        if (!canClick) return;
        canClick = false;
        Close();
        GameController.Instance.Retry();
    }
   
   

}
