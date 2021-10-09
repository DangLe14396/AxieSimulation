using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    static Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }
    public static Vector3 GetMousePosition()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }
    

}
