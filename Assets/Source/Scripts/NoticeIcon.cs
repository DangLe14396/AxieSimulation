using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeIcon : MonoBehaviour
{
    Transform _tranform;
    [SerializeField]
    private bool useOpenningEffect = false,useEffect=true;
    Image icon;
    Color c;
    // Start is called before the first frame update
    void Start()
    {
        _tranform = transform;
        scale = Vector3.one;
        icon = GetComponent<Image>();
        c = icon.color;
        c.a = 0;
        if (useOpenningEffect)
        {
            scale.x = scale.y = 2;
        }
    }

    // Update is called once per frame
    float a = 0;
    Vector3 scale;
    void Update()
    {
        if (useOpenningEffect)
        {
            icon.color = c;
            c.a = Mathf.Lerp(c.a, 1, 0.07f); ;
            _tranform.localScale = scale;
            scale.x = scale.y = Mathf.Lerp(scale.x, 1, 0.07f);
            if (c.a > 0.99f)
            {
                useOpenningEffect = false;
            }
        }
        else
        {
            if (useEffect)
            {
                _tranform.localScale = scale;
                scale.x = scale.y = 1 + Mathf.Sin(a) / 14f;
                a += Mathf.PI / 20;
            }
        }
    }
}
