using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public LevelMap selectedLevelMap;
    public int currentLevel = 0, currentMode = -1;
    [SerializeField]
    private int[] totalLevels = { 275, 70, 70, 60, 60 };
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public LevelMap GetMap(int level, GameMode mode)
    {

        return new LevelMap();

    }
    bool isInit = false;
    public int GetTotalLevel(int mode = 0)
    {
        return totalLevels[mode];
    }

    public void Init()
    {
        if (isInit) return;
        isInit = true;
    }
    public void Clear()
    {
        currentMode = 0;
        currentLevel = 9999;
        selectedLevelMap = null;
    }

    public void SetUp(int level, int mode)
    {

        Debug.Log("LEVEL MANAGER: " + level + " " + mode);

        if ((currentMode != mode) || (currentLevel != level && currentMode == mode))
        {
            if (selectedLevelMap != null)
            {
                Destroy(selectedLevelMap.gameObject);
            }
            GameObject levelObj = Instantiate(Resources.Load(((GameMode)mode).ToString() + "/Level " + level, typeof(GameObject)), transform) as GameObject;
            selectedLevelMap = levelObj.GetComponent<LevelMap>();
            selectedLevelMap.Init();
        }

        currentLevel = level;
        currentMode = mode;

        selectedLevelMap.SetUp(level);

        selectedLevelMap.Show();
    }


}
