using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackGroundManager : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer backGroundImg,backLightImg, backLight2Img;
    [SerializeField]
    private Sprite[] bgSprites,backLightSprites, backLight2Sprites;
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }
   
    public void SetUp()
    {
        int index = ((GameController.Instance.currentLevel-1) / 8) % bgSprites.Length;

        Set(index);
    }
    public void Set(int id)
    {
        backGroundImg.sprite = bgSprites[id];
        backLightImg.sprite = backLightSprites[id];
        backLight2Img.sprite = backLight2Sprites[id];
    }
    
   
}
