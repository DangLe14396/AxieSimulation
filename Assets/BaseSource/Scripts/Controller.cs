using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBaseManager
{
    public static FireBaseManager Instance;
    public void LogEvent(string eventName)
    {

    }
    public void LogLevel(int level, int mode)
    {

    }
    public static string GetLevel(int level)
    {
        return "nun";
    }
}
public class Controller : MonoBehaviour
{
    //1155993084732844
    public static Controller Instance;
    public GameObject pleaseWaitPanel;
    public GameObject gameCam, menuCam;
    public bool isTest = false,testLevel,testAD=false;
    public static string[] MODE_NAMES = {"NORMAL MODE","TREASURE MODE", "TOWER MODE", "BLOCK MODE","SWORD MODE","IQ","IQ" };
    public static int TOTAL_MODE = 4;
    public int iqPack = 0;
    public Material[] enMats, koMats,viMats;
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance == null)
        {
            Instance = this;

           
            StartCoroutine(DelaySetUp());

        }

     

    }
 
    public int GetTotalLevel(GameMode mode=GameMode.Block)
    {
        return GameController.Instance.levelManager.GetTotalLevel((int)mode);
    }
 
    void OnLanguageChanged()
    {
        for (int i = 0; i < MODE_NAMES.Length; i++)
        {
            MODE_NAMES[i] = Wugner.Localize.Localization.GetEntry(null, "Common/Mode" + i).Content;
        }
    }
    public int GetMaxNormalLevel()
    {
        int level = Mathf.Min(PrefInfo.GetTotalUnlockedLevel(0), GameController.Instance.levelManager.GetTotalLevel(0)) - 1;
        return level;
    }
    IEnumerator DelaySetUp()
    {
        yield return null;
        System.GC.Collect();

        SetUp();

    }
    public void PlayChallenge(int level,GameMode mode)
    {
        Debug.Log("LEVEL: " + level+ " "+mode);
        if (level > GameController.Instance.levelManager.GetTotalLevel((int)mode))
        {
            Home.Instance.SetUp();
            Home.Instance.Active();
            Home.Instance.isStarted = false;
            return;
        }

       
        SoundManager.Instance.StopMusic();
        Home.Instance.isStarted = true;


        PrepareLevel(level, mode);

        MenuController.Instance.home.Hide();
        if (PrefInfo.GetTotalUnlockedLevel(0) > 1)
        {
            //AppTrackingListenner.Instance.ShowPopUp();
        }

    }
    
    public void SetUp()
    {
        SetUpHome();  
    }
    public int backHomeCount = 0;
    public void SetUpHome()
    {
        backHomeCount++;
        GameController.Instance.isLevelDone = false;
        MenuController.Instance.SetUp();
        int currentSession = PlayerPrefs.GetInt("CountSession", 1);
       
    }
 
    public void PrepareLevel( int level,GameMode mode)
    {
        //menuCam.SetActive(false);
        //gameCam.SetActive(true);
        SoundManager.Instance.PlaySFX(SFX.openGame, 0.6f);
        //StartCoroutine(DoSetUpGame(level, mode));
        GameController.Instance.SetUp(level, mode, false, true);

    }
    public void ShowLoading()
    {
        StartCoroutine(DoShowLoading());
    }
    [SerializeField]
    private GameObject loading;
    IEnumerator DoShowLoading()
    {
        loading.SetActive(true);
        UnityEngine.UI.Image img = loading.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        Color c = Color.black;
        c.a = 0;
        img.color = c;
        while (c.a < 1)
        {
            c.a += 2 * Time.deltaTime;
            img.color = c;
            yield return null;
        }
        c.a = 1;
        img.color = c;
    }
    IEnumerator DoSetUpGame(int level,GameMode mode)
    {
        yield return null;
    }
    [HideInInspector]
    public  int[,] levelChallengeUnlock = {

        { 12, (18), 24, (30), 36, (42), 48, (54), 9999, (9999),9999} ,
         { 9999,9999,9999,9999,9999,9999,9999,9999,9999,9999,9999},
         { 9999,9999,9999,9999,9999,9999,9999,9999,9999,9999,9999},
         { 9999,9999,9999,9999,9999,9999,9999,9999,9999,9999,9999}




    };
    public int[] iqUnlockLevel = { 150, 200, 250, 999 };
    public int IsAtUnlockLevel(int currentLevel, out bool isFirst,out int index)
    {
        for (int i = 0; i < 1; i++)
        {
            for (int j = 10; j >=0; j--)
            {
                if (currentLevel == levelChallengeUnlock[i, j] )
                {
                    isFirst = j % 2 == 0;
                    index = j;
                    return i;
                   
                }
               
            }
        }
        index = -1;
        isFirst = false;
        return -1;
    }

    public int GetMaxLevel(GameMode currentMode)
    {
        //return 999;
       
        //if( GameController.Instance.levelManager.GetTotalLevel((int)currentMode) == 0)
        //{
        //    return 999;
        //}
      
        int totalLevel = PrefInfo.GetTotalUnlockedLevel(0);

        int total = 0;
        for (int i = 0; i < 11; i++)
        {
            if (totalLevel > levelChallengeUnlock[(int)currentMode-1, i] )
            {
                total += 5;
            }
        }

        total = Mathf.Min(total, GameController.Instance.levelManager.GetTotalLevel((int)currentMode));

        return total;
    }
   
}

internal class Wugner
{
    public static class Localize
    {
        public static class Localization
        {
            public static TempClass GetEntry(TMPro.TextMeshProUGUI text, string n)
            {
                return new TempClass();
            }
        }
    }
}
public class TempClass
{
    public string Content;
}



public enum GameMode
{
    Block, Bonus,Coin, Tower, Goblin, Sword, IQ
}
