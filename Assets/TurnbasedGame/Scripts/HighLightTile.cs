using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightTile : MonoBehaviour
{
    Transform _transform;
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    Animator anim;
  
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }
    void OnHide()
    {
        Hide();
    }
    Color c = Color.white;
    public void SetAlpha(float a)
    {
        c.a = a;
        sr.color = c;
    }
    public void SetUp(Vector3 pos,Color color,float delayShow=0)
    {
        if (_transform == null)
        {
            _transform = transform;
        }
        _transform.localPosition = pos;
        c.a = 0.5f;
        sr.color = color;
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            //Invoke("Show", delayShow);
        }
    }
    public void Show()
    {
        anim.SetTrigger("Open");
    }
    public void PlayClickAnim()
    {
        anim.SetTrigger("Click");
    }
    public void Hide()
    {
        anim.SetTrigger("Hide");

    }
    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}
