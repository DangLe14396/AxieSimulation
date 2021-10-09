using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PausePanel : Panel
{
    public static PausePanel Instance;
    [SerializeField]
    private TextMeshProUGUI levelText;
    public override void PostInit()
    {
        Instance = this;

    }

    public override void Close()
    {
        base.Close();
    }
    float vol;
    public void SetUp()
    {
        Show();
        int level = GameController.Instance.currentLevel;
        levelText.text = "Chapter " + ((level - 1) / 4 + 1) + "-Level " + level;

    }
    public void BackHome()
    {
        Close();
        GameController.Instance.Close();
    }
    public void SelectLevel()
    {
        Close();
        GameController.Instance.ToLevelMenu();
    }
    public void Retry()
    {
        GameController.Instance.Retry();
        Close();
    }

    public void Setting()
    {
        Close();
        //SettingPanel.Instance./SetUp(true);
    }

    public void Resume()
    {
        Close();
    }

}
