using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public int level = 0,mission,round;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private GameObject bossMark;
    [SerializeField]
    private Image panelImg;
    [SerializeField]
    private Sprite lockedSprite, unlockedSprite, currentSprite;
    public bool isLocked = true,isSelected=false;
    private Transform _transform;
    private GameMode mode;
    [SerializeField]
    private Sprite[] frameSprites;
    [SerializeField]
    private Color passedColor, currentColor,lockColor;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void SetUp(int level,GameMode mode,bool isSelected,bool isUnlocked)
    {
        if (Controller.Instance.isTest)
        {
            isUnlocked = true;
        }
        this.level = level;
        this.mode = mode;
        levelText.text = /*isUnlocked ? */(level + 1).ToString()/* : ""*/;

        panelImg.sprite = null;
        panelImg.sprite = isSelected ? currentSprite : (isUnlocked ? unlockedSprite : lockedSprite);
        levelText.color =  (isSelected?currentColor:(isUnlocked ? passedColor : lockColor));
        this.isSelected = isSelected;
        isLocked = !isUnlocked;
        bossMark.SetActive(level>=7 &&(level+1) % 8 == 0);
        if (_transform == null) _transform = transform;
        _transform.localScale = Vector3.one;
    }
    public void Select()
    {
        if (isLocked)
        {
            MessagePanel.Instance.SetUp("Keep playing to unlock this level");
        }
        else
        {
            Debug.Log("ON SELECTED: " + mode + " " + this.level);
            SelectLevelPanel.Instance.OnSelected(this);
           
        }
    }
    float a = 0;
    Vector3 scaleVector = Vector3.one;
    int frame = 0;
    float speedTimer = 0.1f;
    private void Update()
    {
        if (isSelected)
        {
            _transform.localScale = scaleVector;
            scaleVector.x = scaleVector.y = 1+Mathf.Sin(a)/13f;
            a += Mathf.PI / 30f;
            if (frameSprites.Length != 0)
            {
                panelImg.sprite = frameSprites[frame % frameSprites.Length];
                if (speedTimer > 0)
                {
                    speedTimer -= Time.deltaTime;
                }
                else
                {
                    speedTimer = 0.08f;
                    frame++;
                }
            }

        }
    }
}
