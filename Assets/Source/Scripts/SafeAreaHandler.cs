using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaHandler : MonoBehaviour
{
    [SerializeField]
    Rect rect;
    [SerializeField]
    Vector2 min, max;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SAFE AREA: " + Screen.safeArea.size + " ");
        this.rect = Screen.safeArea;
        RectTransform rt = GetComponent<RectTransform>();
        //rt.offsetMin = new Vector2(rt.offsetMin.x, 0);
        //rt.offsetMax = new Vector2(rt.offsetMax.x, -((Screen.height-rect.height)-rect.y)/2f);
        min = rt.offsetMin;
        max = rt.offsetMax;
        rt.offsetMin = new Vector2(rect.x, 0);
        rt.offsetMax = new Vector2(-rect.x,0);

    }

}
