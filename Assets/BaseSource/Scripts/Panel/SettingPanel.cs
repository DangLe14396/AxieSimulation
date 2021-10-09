using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingPanel : Panel
{
    public static SettingPanel Instance;

    public delegate void OnSoundChanged(bool state);
    public static OnSoundChanged onSoundChanged;
    [SerializeField]
    private GameObject randomObj, noRandomObj;
    [SerializeField]
    private GameObject muteObj, unmuteObj;
    [SerializeField]
    private GameObject muteMusicObj, unmuteMusicObj;
    [SerializeField]
    private GameObject vibrationObj, unVibrationObj;

    [SerializeField]
    private GameObject pauseObj, settingsObj;


    [SerializeField]
    private TextMeshProUGUI version,langText;
    [SerializeField]
    private Image flagImg;
    [SerializeField]
    private Sprite[] flagSprites;
    string[] keys = { "en", "vi", "es", "gm", "in", "ita", "jp", "kr", "ru" };
    Dictionary<string, Sprite> flagDict = new Dictionary<string, Sprite>();
    public bool isSFXEnabled = true,isLowSettings=false, isRandom = true;
    int w, h,refresh;
    public override void PostInit()
    {
        Instance = this;

        if (PlayerPrefs.GetInt("RandomLevel", 1) == 1)
        {
            noRandomObj.active = false;
            randomObj.active = true;
            isRandom = true;
        }
        else
        {
            noRandomObj.active = true;
            randomObj.active = false;
            isRandom = false;
        }
        if (PlayerPrefs.GetInt("SoundOn", 1) == 1)
        {
            muteObj.active = false;
            unmuteObj.active = true;
            isSFXEnabled = true;
        }
        else
        {
            muteObj.active = true;
            unmuteObj.active = false;
            isSFXEnabled = false;
        }

        if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
        {
            muteMusicObj.active = false;
            unmuteMusicObj.active = true;
        }
        else
        {
            muteMusicObj.active = true;
            unmuteMusicObj.active = false;
        }

        if (PlayerPrefs.GetInt("Vibration", 1) == 1)
        {
            vibrationObj.active = true;
            unVibrationObj.active = false;
        }
        else
        {
            vibrationObj.active = false;
            unVibrationObj.active = true; 
        }
        string version = SystemInfo.operatingSystem;
        //try
        //{
        //    if (PlayerPrefs.GetInt("FirstSettings", 0) == 0)
        //    {
        //        if (version.Contains("4.0") || version.Contains("4.1") || version.Contains("4.2") || version.Contains("4.3") || version.Contains("4.4") ||
        //            version.Contains("5.0") || version.Contains("5.1") || version.Contains("5.2"))
        //        {
        //            PlayerPrefs.SetInt("LowSettings", 1);
        //        }
        //        PlayerPrefs.SetInt("FirstSettings", 1);

        //    }
        //}
        //catch { }
    
        //for (int i = 0; i < keys.Length; i++)
        //{
        //    flagDict.Add(keys[i], flagSprites[i]);
        //}
        langText.text = PlayerPrefs.GetString("Language", "en").ToUpper();

    }
    void OnLanguageChanged()
    {
        this.version.text = "Version " + Application.version;
        langText.text = PlayerPrefs.GetString("Language", "en").ToUpper();
        //flagImg.sprite = flagDict[PlayerPrefs.GetString("Language", "en")];
    }
    public void SetResolution(int w,int h)
    {
        Screen.SetResolution(w, h, true, refresh);
    }
    IEnumerator DoShowWarning()
    {
        MessagePanel.Instance.SetUp("Game will be restarted to apply new settings");
        while (MessagePanel.Instance.gameObject.activeSelf)
        {
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }
    public void LowSettings()
    {
      
    }

    public delegate void OnSettingChanged();
    public static event OnSettingChanged onChangedListener;
    public bool LowSettingsToggle()
    {
        SoundManager.Instance.PlaySFX(SFX.clickSFX);
        if (PlayerPrefs.GetInt("LowSettings", 0) == 0)
        {
            PlayerPrefs.SetInt("LowSettings", 1);
            isLowSettings = true;
            onChangedListener?.Invoke();
            SetResolution(w / 2, h / 2);
            return false;
        }
        else
        {
            isLowSettings = false;
            PlayerPrefs.SetInt("LowSettings", 0);
            onChangedListener?.Invoke();
            SetResolution(w , h );

            return true;
        }
    }
    public void Sound()
    {
        if (SFXToggle())
        {
            muteObj.active = false;
            unmuteObj.active = true;
        }
        else
        {
            muteObj.active = true;
            unmuteObj.active = false;
        }
    }
   
    public bool SFXToggle()
    {
        if (PlayerPrefs.GetInt("SoundOn", 1) == 1)
        {
            SoundManager.Instance.StopSFX();
            SoundManager.Instance.StopAmbience();
            PlayerPrefs.SetInt("SoundOn", 0);
            isSFXEnabled = false;
            onSoundChanged?.Invoke(isSFXEnabled);
            return false;
        }
        else
        {
            PlayerPrefs.SetInt("SoundOn", 1);
            SoundManager.Instance.PlaySFX(SFX.clickSFX);
            isSFXEnabled = true;
            onSoundChanged?.Invoke(isSFXEnabled);
            return true;
        }
    }

    public void Music()
    {
        Debug.Log("Music");
        if (MusicToggle())
        {
            muteMusicObj.active = false;
            unmuteMusicObj.active = true;
        }
        else
        {
            muteMusicObj.active = true;
            unmuteMusicObj.active = false;
        }
    }
    bool MusicToggle()
    {
        if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
        {
            SoundManager.Instance.PauseMusic();
            PlayerPrefs.SetInt("MusicOn", 0);
            return false;
        }
        else
        {
            SoundManager.Instance.ContinueMusic();
            PlayerPrefs.SetInt("MusicOn", 1);
            return true;
        }

    }

    public void Vibration()
    {
        Debug.Log("Vibration");
        if (VibrationToggle())
        {
            vibrationObj.active = true;
            unVibrationObj.active = false;
        }
        else
        {
            vibrationObj.active = false;
            unVibrationObj.active = true;
        }
    }
    bool VibrationToggle()
    {
        if (PlayerPrefs.GetInt("Vibration", 1) == 1)
        {
            //SoundManager.Instance.PauseMusic();
            PlayerPrefs.SetInt("Vibration", 0);
            return false;
        }
        else
        {
            //SoundManager.Instance.ContinueMusic();
            PlayerPrefs.SetInt("Vibration", 1);
            return true;
        }

    }

    public void Randomlevel()
    {
        if (RandomlevelToggle())
        {
            randomObj.active = true;
            noRandomObj.active = false;
        }
        else
        {
            randomObj.active = false;
            noRandomObj.active = true;
        }
    }

    public bool RandomlevelToggle()
    {
        if (PlayerPrefs.GetInt("RandomLevel", 1) == 1)
        {
            PlayerPrefs.SetInt("RandomLevel", 0);
            SoundManager.Instance.PlaySFX(SFX.clickSFX);
            isRandom = false;
            return false;
        }
        else
        {
            PlayerPrefs.SetInt("RandomLevel", 1);
            SoundManager.Instance.PlaySFX(SFX.clickSFX);
            isRandom = true;
            return true;
        }
    }

    public override void Deactive()
    {
        base.Deactive();
        Home.Instance.isStarted = false;
        //Time.timeScale = 1;
        GameUI.Instance.canBack = true;
        Home.Instance.isQuitActive = false;


    }
    public void ToHome()
    {
        Close();
        GameController.Instance.Close();
    }
    string level = "";
    private void OnGUI()
    {
        if (Controller.Instance.isTest)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("LEVEL: ", GUILayout.Width(100), GUILayout.Height(50));
            level = GUILayout.TextField(level, GUILayout.Width(150), GUILayout.Height(50));
            if (GUILayout.Button("APPLY", GUILayout.Width(100), GUILayout.Height(100)))
            {
                Close();
                //int l = int.Parse(level);
                int l = Mathf.Min(int.Parse(level), Controller.Instance.GetTotalLevel());
            
                GameController.Instance.SetUp(l,GameController.Instance.currentMode, false, false);
            }
           
            GUILayout.EndHorizontal();
        }

    }
    public void SetUp(bool isPaused=false)
    {
        if (Home.Instance.isStarted) return;
        Home.Instance.isStarted = false;
        if (GameController.Instance.isLevelDone) return;
        //Time.timeScale = 0;
        Show();
        GameUI.Instance.canBack = false;
        Home.Instance.isQuitActive = true;
        this.version.text = "Version " + Application.version;
        pauseObj.SetActive(isPaused);
        settingsObj.SetActive(!isPaused);

    }
    public void Retry()
    {
        Close();
        GameUI.Instance.Retry();
    }
    
    public override void ShowAfterAd()
    {
        Show();
    }
    public void ShowRestoreSuccess()
    {
        Invoke("DoShow", 0.6f);
    }
    void DoShow()
    {
        MessagePanel.Instance.SetUp("Your purchased have been restores successfully!");
    }

}
