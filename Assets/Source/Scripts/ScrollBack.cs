using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBack : MonoBehaviour
{

    public static ScrollBack Instance;
    Camera cam;
    float boundaryX = 18, boundaryY = 14.65f;
    float defaultZoom = 0, defaultCamSize = 7.84f;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        cam = Camera.main;
        defaultZoom = cam.orthographicSize;
    }
    public Vector2 GetPos(Vector2 pos)
    {
        float ratio = 1 / (cam.orthographicSize / defaultCamSize);
        float screenRatio = 1f * Screen.width / Screen.height;
        float h = cam.orthographicSize;
        float w = h * screenRatio;
        float bX = boundaryX - w;
        float bY = boundaryY - h;
        pos.x = Mathf.Clamp(pos.x, -bX, bX);
        pos.y = Mathf.Clamp(pos.y, -bY, bY);

        return pos;
    }
    public bool active = true;

    public bool isDown = false;
    Vector2 dir,pos;
    float timer = 1;
    Vector2 posA, posB;
    bool first = false;
    [SerializeField]
    float firstDist = 0,currentSize=0;
    // Update is called once per frame
    void Update()
    {
        if (!active) return;
       
        if (isDown)
        {
            Vector2 currentPos = cam.ScreenToViewportPoint(Input.mousePosition);
            pos = oPos + -(currentPos - startPos) * 14;
            //dir = (currentPos - startPos).normalized;
            float ratio = 1/(cam.orthographicSize / defaultCamSize);
            float screenRatio = 1f*Screen.width / Screen.height;
            float h = cam.orthographicSize;
            float w = h * screenRatio;
            float bX = boundaryX - w;
            float bY = boundaryY - h;
            pos.x = Mathf.Clamp(pos.x, -bX,bX);
            pos.y = Mathf.Clamp(pos.y, -bY,bY);
            CameraShake.Instance.GetTransfrom().localPosition = Vector3.MoveTowards(CameraShake.Instance.GetTransfrom().localPosition,pos,2f);

            GameManager.Instance.mainMap.HighLightTile(CameraShake.Instance.GetTransfrom().localPosition);
        }

       
           

    }
    bool init = false;
    private void OnDisable()
    {
        if(cam!=null)
        cam.orthographicSize = defaultZoom;
        active = true;
        isDown = false;
    }
    private void OnEnable()
    {
        init = true;

    }
    public bool IsDrag()
    {
        return isDown;
    }

    Vector2 startPos,oPos;
    public void OnMouseDown()
    {
        if (!active) return;
        startPos = cam.ScreenToViewportPoint(Input.mousePosition);
        oPos = CameraShake.Instance.GetTransfrom().localPosition;
        isDown = true;
    }
    private void OnMouseUp()
    {
        if (!active) return;
        if (isDown)
        {
            oPos = CameraShake.Instance.GetTransfrom().localPosition;
            timer = 1;
            isDown = false;
        }
    }
}
