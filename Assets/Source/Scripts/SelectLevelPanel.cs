using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class SelectLevelPanel : Panel
{
    public static SelectLevelPanel Instance;
    public override void PostInit()
    {
        Instance = this;
    }

    [SerializeField]
    private LevelButton[] buttons;

    [SerializeField]
    private TextMeshProUGUI nextText, previousText, pageText;
    [SerializeField]
    private GameObject nextBtn, prevBtn;
    int levelPerPage = 12;
    [SerializeField]
    int current = 0;
    public override void Deactive()
    {
        base.Deactive();
        Home.Instance.isQuitActive = false;
        Home.Instance.isStarted = false;
    }

    public void SetUp()
    {
        if (Home.Instance.isStarted) return;
        Home.Instance.isStarted = true;
        Home.Instance.isQuitActive = true;
        int l = Mathf.Min(PrefInfo.GetTotalUnlockedLevel(0), Controller.Instance.GetTotalLevel()) - 1;
        int totalLevel = Controller.Instance.GetTotalLevel();
        //current = Mathf.Min((Controller.Instance.GetLevel(l, PrefInfo.GetRound(l + 1, 0)) -1)/levelPerPage, (((totalLevel - 1) / levelPerPage) ));

        int level = Mathf.Min(PrefInfo.GetTotalUnlockedLevel(0), Controller.Instance.GetTotalLevel());
        current = (level - 1) / levelPerPage;
        //Debug.LogError("CURRNE: " + Controller.Instance.GetLevel(l, PrefInfo.GetRound(l + 1, 0)) + " " + current);
        //current = (Mathf.Min(PrefInfo.GetTotalUnlockedLevel(0), GameController.Instance.levelManager.GetTotalLevel((int)0) - 1)) / levelPerPage;
        SetText(0);
        //pageText.font = Wugner.Localize.Localization.CurrentDefaultFont;
        PrepareButton(0);
        
        Show();
        FireBaseManager.Instance.LogEvent("PANEL_SelectLevel_Open");
    }

    public void PrepareButton(GameMode mode = 0)
    {
        //int totalLevel = GameController.Instance.levelManager.GetTotalLevel((int)mode);
        int totalLevel = Controller.Instance.GetTotalLevel();
        int l = Mathf.Min(PrefInfo.GetTotalUnlockedLevel(0), Controller.Instance.GetTotalLevel()) - 1;
        int currentLevel = PrefInfo.GetTotalUnlockedLevel(0) ;
        for (int i = 0; i < buttons.Length; i++)
        {
            int lv = (current * levelPerPage + i);
            if (lv + 1 > totalLevel)
            {
                buttons[i].gameObject.SetActive(false);
            }
            else
            {
                //Debug.Log("BUTTON: " + (lv + 1) + " :" + Mathf.Min(currentLevel, GameController.Instance.levelManager.GetTotalLevel(0)));
                buttons[i].SetUp(lv, mode, 
                    (lv + 1) == Mathf.Min(currentLevel, totalLevel)
                    , (lv + 1) <= Mathf.Min(currentLevel, totalLevel));
                buttons[i].gameObject.SetActive(true);
            }
        }
    }
    Vector2 pos;
    private void Update()
    {
        if (Input.touchCount>0)
        {
            Touch start = Input.GetTouch(0);
            if (start.phase == TouchPhase.Began)
            {
                pos = start.position;
            }
            else if (start.phase == TouchPhase.Ended)
            {
                Vector2 dir=pos-start.position;
                float dist = (pos.x - start.position.x);
                dist = dist < 0 ? -dist : dist;
                if (dist > 30f)
                {
                    if (dir.x > 0)
                    {
                        Next();
                    }
                    else
                    {
                        Previous();
                    }
                }
            }
        }
    }
    void SetText(GameMode mode = 0)
    {
        //int totalLevel = GameController.Instance.levelManager.GetTotalLevel((int)mode);
        int totalLevel = Controller.Instance.GetTotalLevel();

        nextBtn.SetActive(totalLevel > levelPerPage);
        prevBtn.SetActive(totalLevel > levelPerPage);

        pageText.text = /*string.Format(Wugner.Localize.Localization.GetEntry(pageText,Loc.ID.Common.PageText).Content,(current+1))*/current+1+"/"+(((totalLevel-1)/levelPerPage)+1);



    }
    public void Next()
    {
        current++;
        int totalLevel = Controller.Instance.GetTotalLevel();
        //int totalLevel = GameController.Instance.levelManager.GetTotalLevel((int)0);
        current = ((current) > (totalLevel - 1) / levelPerPage ? 0 : current);
        PrepareButton();
        SetText();
    }
    public void Previous()
    {
        current--;
        int totalLevel = Controller.Instance.GetTotalLevel();
        //int totalLevel = GameController.Instance.levelManager.GetTotalLevel((int)0);
        current = ((current) < 0 ? (totalLevel - 1) / levelPerPage : current);
        PrepareButton(0);
        SetText(0);
    }
    public void OnSelected(LevelButton button)
    {
        Close();

        Controller.Instance.PlayChallenge(button.level+1, 0);
    }
}
