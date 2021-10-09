using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap_Mark : MonoBehaviour
{
    RectTransform _transform;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private Image iconImg;
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetUp(CharacterController cc)
    {
        if (_transform == null)
        {
            _transform = GetComponent<RectTransform>();
        }
        this.cc = cc;
        cc.miniMapMarker = this;
        iconImg.sprite = sprites[(int)cc.characterType];

        UpdatePosition();
        Show();
    }
    public void UpdatePosition()
    {
        Vector2 pos = cc.transform.localPosition;
        _transform.anchoredPosition = pos*20;
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
