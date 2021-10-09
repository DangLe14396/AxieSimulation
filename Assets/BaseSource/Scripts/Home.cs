using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Home : MonoBehaviour
{
    public static Home Instance;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private RectTransform content;
    // Start is called before the first frame update
    public void PostInit()
    {
        Instance = this;
    }
    void OnLanguageChanged()
    {
    }
    public bool isStarted = false;
    public void PlayNow()
    {
        if (isStarted) return;
        isStarted = true;
        int level = PrefInfo.GetTotalUnlockedLevel(0);
        Controller.Instance.PlayChallenge(level, 0);
    }
    public void ActiveStressTest()
    {
        if (isStarted) return;
        isStarted = true;
        Controller.Instance.PlayChallenge(10, 0);
    }
    private void OnEnable()
    {
        
    }

    public void SetUp()
    {
        SoundManager.Instance.PlayMusic(SFX.theme,1f);
        SoundManager.Instance.StopAmbience();
       

     
        isStarted = false;


    }
   
    public void JustHide()
    {
        if (gameObject.activeSelf)
            GetComponent<Animator>().SetTrigger("Close");
    }
    public void Hide()
    {
        if (gameObject.activeSelf)
        GetComponent<Animator>().SetTrigger("Close");
       

    }
    public void Show()
    {
        Active();
    }
    public void Deactive()
    {
        gameObject.SetActive(false);
        isStarted = false;
    }
    bool isTrial = false;
    float lastTime = 0;
    public void Active()
    {
        gameObject.SetActive(true);
        isStarted = false;
    }
   
    public bool isQuitActive=false;

}
