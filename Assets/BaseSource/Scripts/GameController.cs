using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public LevelManager levelManager;
    public int currentLevel = 0;
    public GameMode currentMode = 0;
    public int currentEarnedCoin = 0, currentEarnedGem = 0;
    public int playCount = 0, levelStarCount = 0;
    public const int REFILLTIME = 30;
    public bool isInstantWin = false;
    public bool isPlayRandom = false;
    public int victoryTeam = 0;
    // Start is called before the first frame update
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            Init();
            SettingPanel.onChangedListener += OnChanged;

        }

    }
    public void Init()
    {
        levelManager.Init();
    }

    void OnChanged()
    {
    }

    public void SetUp(int level, GameMode mode, bool isFromMenu, bool delay)
    {
        System.GC.Collect();
        isInstantWin = false;
        OnChanged();
        if (level < PrefInfo.GetTotalUnlockedLevel(mode))
        {
            isReplayLevel = true;
        }
        else
        {
            isReplayLevel = false;
        }
        Debug.Log("SET UP GAME " + level + " " + mode);
        CameraShake.Instance.Restore();
        if (levelManager.selectedLevelMap != null)
        {
            Clear();
        }
        PrepareLevel(level, mode);
        //SoundManager.Instance.PlayMusic(SFX.theme, 0.35f);

      
    }

    public void PrepareLevel(int level, GameMode mode)
    {
        Clear();
        isLevelDone = false;
        currentEarnedCoin = 0;
        currentEarnedGem = 0;

        if ((currentLevel != level && currentMode == mode) || currentMode != mode)
        {
            levelStarCount++;
        }
        this.currentLevel = level;

        this.currentMode = mode;

        Debug.Log("PREARE LEVEL " + level);
        levelManager.SetUp(level, (int)mode);
        GameUI.Instance.SetUp(level, mode);

        playCount++;
    }
    public void ActiveUI()
    {
        GameUI.Instance.Active();
    }
    public void Retry()
    {
        SetUp(currentLevel, currentMode, false, false);
        currentEarnedCoin = 0;
        currentEarnedGem = 0;
    }
    public void NextLevel()
    {
        Debug.Log("NEXT LEVEL");
        if (currentLevel >= levelManager.GetTotalLevel())
        {
            Close();
        }
        else
        {
            SetUp(currentLevel + 1, currentMode, false, false);
        }
        currentEarnedCoin = 0;
        currentEarnedGem = 0;
    }

   

    public void Close()
    {
        Time.timeScale = 1;
     
        if (levelManager.selectedLevelMap != null)
        {
            HighLightSpawner.Instance.ClearAll();
            HighLightMap.Instance.Hide();
            levelManager.selectedLevelMap.Deactive();
            levelManager.Clear();
        }
        GameUI.Instance.Close();
        Clear();
        Controller.Instance.SetUpHome();
    }
    IEnumerator DoSetUpHome()
    {
        yield return new WaitForSeconds(0.1f);
        Controller.Instance.SetUpHome();

    }
    public void ToLevelMenu()
    {
        Time.timeScale = 1;
        GameUI.Instance.Close();
        Clear();
        //LevelSelectPanel.Instance.SetUp();
    }
    public void Clear()
    {
        GameUI.Instance.Deactive();
        if (levelManager.selectedLevelMap != null)
            levelManager.selectedLevelMap.Deactive();

    }

    public bool isLevelDone = false;
 
    public bool isReplayLevel = false;
    public void FinishLevel()
    {
        if (isLevelDone) return;
        isLevelDone = true;

        EffectManager.Instance.PlayFireWork2();
        string levelStats = PrefInfo.GetLevelStats(currentLevel, currentMode);
        int levelStar = int.Parse(levelStats[0].ToString());
        bool levelState = levelStats[1] == '1';
        Debug.Log("FINISH LEVEL " + currentLevel);
        if (levelStar == 0)
        {
          
            isReplayLevel = false;
            PrefInfo.SetRewardProgress(PrefInfo.GetRewardProgress() + 1);
            PrefInfo.SetTotalUnlockedLevel(PrefInfo.GetTotalUnlockedLevel(currentMode) + 1, currentMode);
        }
        else
        {
            isReplayLevel = true;
        }
        PrefInfo.SetLevelStats(currentLevel, currentMode, Mathf.Max(levelStar, 3), true);

        string nextLevelStats = PrefInfo.GetLevelStats(currentLevel + 1, currentMode);
        int nextLevelStar = int.Parse(nextLevelStats[0].ToString());
        PrefInfo.SetLevelStats(currentLevel + 1, currentMode, Mathf.Max(nextLevelStar, 0), true);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.soundPrefab.GetWinClip(), 0.8f);
     
        GameUI.Instance.ShowResult(currentLevel, currentEarnedCoin, currentEarnedGem);


        SoundManager.Instance.StopMusic();


    }


  
    public void GameOver()
    {
        if (isLevelDone) return;
        isLevelDone = true;
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlaySFX(SoundManager.Instance.soundPrefab.GetLoseClip(), 0.8f);
        GameUI.Instance.ShowResultFail(currentLevel, currentEarnedCoin);


    }
   
}
