using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap : MonoBehaviour
{
    public int totalMonster = 0,totalKill=0;
    public int mission, missionLevel;
    public int level = -1;
    public GameObject content, tempMap, cloneMap, dynamicObj, dynamicContent, cloneDynamicObj;
    protected bool isAttached = false;
    public LevelMap next;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void AddMonster()
    {
        totalMonster++;
    }
    public int GetTotalMonster()
    {
        return totalMonster;
    }
    public virtual void OnDrag() { }
    public virtual void ShowHint()
    {

    }
    protected bool canPlaySFX = true;
    protected void  ResetPlaySFX()
    {
        canPlaySFX = true;
    }
    public void DestroyCoin()
    {
    }
    protected int t = 0;
    public virtual void Attach(GameObject content)
    {
        if (tempMap == null)
        {
            tempMap = transform.GetChild(0).gameObject;
            tempMap.SetActive(false);
        }

        this.content = Instantiate(tempMap, transform);

        this.content.name = "map" + t;
        t++;
        this.content.SetActive(true);
        isAttached = true;
    }
    public void MakeClone()
    {
        if (cloneMap != null)
        {
            Destroy(cloneMap);
        }
        if (cloneDynamicObj != null)
        {
            Destroy(cloneDynamicObj);
        }
        cloneMap = Instantiate(content, transform);
        cloneDynamicObj = Instantiate(dynamicContent, transform);
        cloneMap.SetActive(false);
        cloneDynamicObj.SetActive(false);
    }
    protected bool isInit = false;
    public virtual void Init()
    {
        if (isInit) return;
        isInit = true;
       
    }
    public virtual void Check()
    {
       
    }
    public virtual void Finish()
    {
        if (isFinished) return;
        isFinished = true;
        GameController.Instance.FinishLevel();
    }
   
    public bool isFinished = false;
    public virtual void Lose()
    {
        if (isFinished) return;
        isFinished = true;
    }
    public virtual void SetUp(int level)
    {
        Debug.Log("SETUP " + gameObject.name);
        this.level = level;
        HighLightSpawner.Instance.ClearAll();
        HighLightMap.Instance.Hide();
        Active();

    }
    public virtual void Show()
    {
        Active();
    }
    
    public virtual void Deactive()
    {
        Debug.Log("DEACTIVE LEVELMAP: " + gameObject.name);
        gameObject.SetActive(false);
       
    }
    public void Active()
    {
        gameObject.SetActive(true);
    }
  
  
  
}

