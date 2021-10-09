using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    public static CameraShake Instance;
    [SerializeField]
    private Transform[] cams;
    private Transform _transform;
    Camera[] cameras;
    [HideInInspector]
    private float shakeTimer, shakePower;
    Vector3 defaultPos;
    [SerializeField]
    private float sceneWidth = 5;
    [SerializeField]
    float ratio;
    // Use this for initialization
    public float GetCurrentSize()
    {
        return cameras[0].orthographicSize;
    }
    void Start()
    {
         cameras = new Camera[cams.Length];
        for(int i = 0; i < cameras.Length; i++)
        {
            cameras[i] = cams[i].GetComponent<Camera>();
        }
        _transform = transform;
        Instance = this;
        defaultPos = cams[0].localPosition;

        float unitsPerPixel = sceneWidth / Screen.width;

        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        if (Screen.width * 1f / Screen.height >= 0.73f && Screen.width * 1f / Screen.height <= 0.77f)
        {
            desiredHalfHeight = 6.31f;
        }
        cameras[0].orthographicSize = desiredHalfHeight;
        defaultSize = desiredHalfHeight;
        ratio = Screen.width*1f / Screen.height;

    }
    public void SetActiveSecondCamera(bool active)
    {
        cameras[1].gameObject.SetActive(active);
    }
    public void Restore()
    {
        StopAllCoroutines();
        _transform.position = Vector3.zero;

        float unitsPerPixel = sceneWidth / Screen.width;

        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        if (Screen.width * 1f / Screen.height >= 0.65f && Screen.width * 1f / Screen.height <= 0.77f)
        {
            desiredHalfHeight = 6.31f;
        }
        defaultSize = desiredHalfHeight;

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].orthographicSize = defaultSize;
        }
        zoomScale = defaultSize;
    }
    [SerializeField]
    float left, right,top,bottom;
    public void RegisterBound(float left, float right,float top,float bottom)
    {
        this.left = left;
        this.right = right;
        this.top = top;
        this.bottom=Mathf.Min(bottom,-5f);
    }
    public void MoveToPoint(Vector2 pos)
    {
        float camSize = cameras[0].orthographicSize;
        float camSize2 = Mathf.Min(camSize, (right - left) / (ratio * 2f));

        pos.x = Mathf.Clamp(pos.x, left + ratio * camSize2, right - ratio * camSize2);
        pos.y = Mathf.Clamp(pos.y, bottom + camSize, top - camSize);
        _transform.position =  pos;
    }
    public void MoveToValidPoint(Vector2 pos)
    {
        float camSize = cameras[0].orthographicSize;
        float camSize2 = Mathf.Min(camSize, (right - left) / (ratio * 2f));
        
        pos.x = Mathf.Clamp(pos.x, left + ratio * camSize2, right - ratio * camSize2);
        pos.y = Mathf.Clamp(pos.y, bottom + camSize, top - camSize);
        _transform.position = Vector3.Lerp(_transform.position, pos, isZoomIn ? 1 : 0.2f);
    }
    public void Move(Vector2 delta)
    {
        //if (isZoomIn) return;
        bool isZoomIn = false;
        float camSize = cameras[0].orthographicSize;
        float camSize2 = Mathf.Min(camSize, (right - left) / (ratio * 2f));

        Vector2 pos = (Vector2)_transform.position+ delta.normalized*delta.magnitude/50;
        pos.x = Mathf.Clamp(pos.x, left + ratio * camSize2, right - ratio * camSize2);
        pos.y = Mathf.Clamp(pos.y, bottom + camSize, top - camSize);
        _transform.position = Vector3.Lerp(_transform.position, pos, isZoomIn?1:0.23f);
    }
    public void Zoom(float size)
    {
        StopAllCoroutines();
        StartCoroutine(DoZoom(size));
    }
    public void ZoomIn(Vector3 pos,float size)
    {
        StopAllCoroutines();
        StartCoroutine(DoZoomIn(pos,size));
    }
    public void ZoomOut()
    {
        StopAllCoroutines();
        StartCoroutine(DoZoomOut());
    }
    IEnumerator DoZoom(float toSize)
    {
        isZoomIn = true;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        float camSize, camSize2;
        while (Mathf.Abs(cameras[0].orthographicSize - toSize) > 0.05f /*|| Vector3.Distance(pos,_transform.position)>0.03f*/)
        {

            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].orthographicSize = Mathf.Lerp(cameras[i].orthographicSize, toSize, 0.03f);
            }
            camSize = cameras[0].orthographicSize;
            camSize2 = Mathf.Min(camSize, (right - left) / (ratio * 2f));
            if (camSize2 * ratio * 2f >= right - left - 0.02f)
            {
                camSize2 = Mathf.Min(camSize2, (right - left) / (ratio * 2f));
                for (int i = 0; i < cameras.Length; i++)
                {
                    cameras[i].orthographicSize = camSize2;
                }
                break;
            }
            if (camSize * 2f >= top - bottom - 0.02f)
            {
                camSize2 = Mathf.Min(camSize2, (top - bottom) / (2f));
                for (int i = 0; i < cameras.Length; i++)
                {
                    cameras[i].orthographicSize = camSize2;
                }
                break;
            }
            yield return wait;
        }
        isZoomIn = false;

        camSize = cameras[0].orthographicSize;
        camSize2 = Mathf.Min(camSize, (right - left) / (ratio * 2f));
        if (camSize2 * ratio * 2f >= right - left - 0.02f)
        {
            camSize2 = Mathf.Min(camSize2, (right - left) / (ratio * 2f));
        }
        if (camSize * 2f >= top - bottom - 0.02f)
        {
            camSize2 = Mathf.Min(camSize2, (top - bottom) / (2f));
        }
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].orthographicSize = camSize2;
        }
    }
    Vector2 zoomPos;
    float defaultSize = 5;
    bool isZoomIn = false;
    IEnumerator DoZoomIn(Vector3 pos,float toSize)
    {

        isZoomIn = true;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        float camSize, camSize2;
        while (Mathf.Abs(cameras[0].orthographicSize- toSize)>0.05f /*|| Vector3.Distance(pos,_transform.position)>0.03f*/)
        {
            
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].orthographicSize = Mathf.Lerp(cameras[i].orthographicSize, toSize, 0.05f);
            }
             camSize = cameras[0].orthographicSize;
             camSize2 = Mathf.Min(camSize, (right - left) / (ratio * 2f));
            //pos.x = Mathf.Clamp(pos.x, left + ratio * camSize, right - ratio * camSize);
            //_transform.position = Vector3.Lerp(_transform.position, pos, 1f);
            if (camSize2*ratio*2f>=right-left-0.02f)
            {
                camSize2 = Mathf.Min(camSize2, (right - left) / (ratio * 2f));
                for (int i = 0; i < cameras.Length; i++)
                {
                    cameras[i].orthographicSize = camSize2;
                }
                break;
            }
            if (camSize  * 2f >= top -bottom - 0.02f)
            {
                camSize2 = Mathf.Min(camSize2, (top -bottom) / ( 2f));
                for (int i = 0; i < cameras.Length; i++)
                {
                    cameras[i].orthographicSize = camSize2;
                }
                break;
            }
            yield return wait;
        }
        zoomPos = pos;
        isZoomIn = false;

         camSize = cameras[0].orthographicSize;
         camSize2 = Mathf.Min(camSize, (right - left) / (ratio * 2f));
        if (camSize2 * ratio * 2f >= right - left - 0.02f)
        {
            camSize2 = Mathf.Min(camSize2, (right - left) / (ratio * 2f));
        }
        if (camSize * 2f >= top - bottom - 0.02f)
        {
            camSize2 = Mathf.Min(camSize2, (top - bottom) / (2f));
        }
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].orthographicSize = camSize2;
        }
    }
    IEnumerator DoZoomOut()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        while (Mathf.Abs(cameras[0].orthographicSize - defaultSize) > 0.05f)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].orthographicSize = Mathf.Lerp(cameras[i].orthographicSize, defaultSize, 0.05f);
            }
            yield return wait ;
        }
        //for (int i = 0; i < cameras.Length; i++)
        //{
        //    cameras[i].orthographicSize = defaultSize;
        //}
    }
    public void Shake(float shakePower, float shakeTimer)
    {
        this.shakePower = shakePower;
        this.shakeTimer = shakeTimer;
    }
    public bool isShaking()
    {
        if (shakeTimer > 0)
        {
            return true;
        }
        return false;
    }
    void ShakeScreen()
    {
        if (shakeTimer >= 0)
        {

            Vector2 shakePos = Random.insideUnitSphere * shakePower;

            for(int i = 0; i < cams.Length; i++)
            {
                cams[i].transform.position = new Vector3(cams[i].position.x + shakePos.x, cams[i].position.y + shakePos.y, cams[i].position.z);

            }

            shakeTimer -= Time.deltaTime;
        }
        if (shakeTimer < 0)
        {
            if (cams[0].localPosition.x != 0)
            {
                for (int i = 0; i < cams.Length; i++)
                {
                    cams[i].localPosition = Vector3.Lerp(cams[i].localPosition, defaultPos, 0.5f);
                }
               
            }
        }

    }
    private Transform target,forceTarget;
    bool isFollowingTarget = false;
    public void FocusOnTarget(Transform target)
    {
        this.target = target;
        SetFocusTarget(true);
        
    }
    public void ForceFocusOnTarget(Transform target)
    {
        this.forceTarget = target;
        SetFocusTarget(true);
    }
    public void ClearForceFocus()
    {
        this.forceTarget = null;
    }
    public void SetFocusTarget(bool active)
    {
        isFollowingTarget = active;
    }
    public void Follow(Vector2 pos,float speed=0.25f)
    {
        float camSize = cameras[0].orthographicSize;
        //pos.x = Mathf.Clamp(pos.x,left+ratio*camSize,right-ratio*camSize);
        _transform.localPosition = Vector2.Lerp(_transform.localPosition, pos, speed);
    }
    float zoomScale = 0;
    float scroll = 0;
    float scrollSpeed = 0.9f;
    void ZoomHandler()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            zoomScale += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            zoomScale = Mathf.Clamp(zoomScale, 2, 6);
        }

            cameras[0].orthographicSize =Mathf.Lerp(cameras[0].orthographicSize,zoomScale,0.1f);
    }

    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.levelManager.selectedLevelMap != null)
        {
            ShakeScreen();

            ZoomHandler();
            if (!ScrollBack.Instance.IsDrag())
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    if (isFollowingTarget)
                    {
                        Follow(forceTarget==null?target.localPosition:forceTarget.localPosition, 0.05f);
                    }
                }
            }
            else
            {
                timer = 2;
            }

            HighLightMap.Instance.OnUpdate(_transform.localPosition);

        }
    }

    public Transform GetTransfrom()
    {
        return _transform;
    }
}
