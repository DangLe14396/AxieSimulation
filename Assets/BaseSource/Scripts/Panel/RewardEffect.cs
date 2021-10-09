using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardEffect : MonoBehaviour
{

    public static RewardEffect Instance;
    [SerializeField]
    private Transform holderCoin,holderGem;
    [SerializeField]
    private Transform[] targets;
    private GameObject prefabCoin,prefabGem,prefabSword;
    [SerializeField]
    private List<Image> listImg;
    private List<Transform> listCoinTransform,listGemTransform;
    [SerializeField]
    private Sprite [] sprites;
    public bool isActive = false;
    [SerializeField]
    private ParticleSystem coinPS;
    [SerializeField]
    private TMPro.TextMeshPro amountText;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        holderCoin = transform.GetChild(0);
        prefabCoin = transform.GetChild(1).gameObject;
        prefabGem = transform.GetChild(2).gameObject;
        prefabSword = transform.GetChild(3).gameObject;
        listImg = new List<Image>();
        listCoinTransform = new List<Transform>();
        foreach(Transform t in holderCoin)
        {
            listCoinTransform.Add(t);
            listImg.Add(t.GetComponentInChildren<Image>());
        }
    }
    //#if UNITY_EDITOR
    //    private void OnGUI()
    //    {
    //        if(GUILayout.Button("TEST COIN FLY",new GUILayoutOption(GUILayout.))
    //        {
    //            SetUp(20, targets[2], true, 0);
    //        }
    //    }
    //#endif
    public void ShowCoinPS(float amount = 100)
    {
        amountText.text = "+" + amount + "<sprite=0>";
        coinPS.Play();
        StartCoroutine(DoShowCoinPS());
    }
    Color c;
    IEnumerator DoShowCoinPS()
    {
        c = amountText.color;
        c.a = 0;
        amountText.color = c;
        Transform t = amountText.transform;
        Vector3 scale = new Vector3(4,4,1);
        t.localScale = new Vector3(0, 0, 1);
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        while (c.a < 0.98f)
        {
            c.a = Mathf.Lerp(c.a, 1, 0.25f);
            t.localScale = Vector3.Lerp(t.localScale, scale, 0.25f);
            amountText.color = c;
            yield return wait;
        }
        float a = 0;
        float s = t.localScale.x;
        while (a < Mathf.PI)
        {
            t.localScale = Vector3.Lerp(t.localScale, scale, 0.25f);
            scale.x = scale.y = s + Mathf.Sin(a) / 30;
            a += Mathf.PI / 45;
            yield return wait;
        }
        scale.x = scale.y = 1.5f;
        while (c.a > 0.02f)
        {
            c.a = Mathf.Lerp(c.a, 0, 0.2f);
            amountText.color = c;
            t.localScale = Vector3.Lerp(t.localScale, scale, 0.25f);

            yield return wait;
        }
        c.a = 0;
        amountText.color = c;
    }
    [SerializeField]
    private Canvas canvas;
    public void SetUp(int amount, Transform from, bool quick = false,int target=0,float delay=0,int sprite=0)
    {
        if (isActive) return;
        isActive = true;
        Clean();
        amount = Mathf.Min(10, amount);
        //Debug.Log("FROM " + from + " " + targets[target].position);
        //from += CameraFlow.Instance.transform.localPosition;
        //from=CanvasExtensions.WorldToCanvas(canvas, from, Camera.main);
        StartCoroutine(DoGenerate(amount, from, targets[target], quick,delay,sprite));
    }
   
    public void Clean()
    {
        StopAllCoroutines();
        for (int i = 0; i < listCoinTransform.Count; i++)
        {
            listCoinTransform[i].gameObject.SetActive(false);
        }
    }
    Transform Get(int i)
    {
        try
        {
            return listCoinTransform[i];
        }
        catch
        {
            GameObject o = Instantiate(prefabCoin, holderCoin);
            listCoinTransform.Add(o.transform);
            listImg.Add(o.GetComponentInChildren<Image>());
            return Get(i);

        }
    }
    IEnumerator DoGenerate(int amount,Transform from, Transform toPos,bool quick,float delay=0,int sprite=0)
    {
        yield return new WaitForSecondsRealtime(delay);
        float time=(quick?0.3f:0.9f)/amount;
        //Debug.Log("COIN: " + from.position + " " + toPos.position);
        for (int i = 0; i < amount; i++)
        {
            Transform t = Get(i);
            Image img = listImg[i];
            img.sprite = sprites[sprite];
            StartCoroutine(DoPopUp(i,from,toPos));
            yield return new WaitForSecondsRealtime(time);
        }

        isActive = false;

    }
    private Vector3 CalculatePoint(float t,Vector3 a,Vector3 b,Vector3 o)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * a;
        p += 2 * u * t * b;
        p += tt * o;
        return p;
    }
    IEnumerator DoPopUp(int index,Transform from,Transform toPos)
    {
        Transform t = listCoinTransform[index];
        t.gameObject.SetActive(true);
        t.position = from.position+(Vector3)Random.insideUnitCircle*0.1f;

        Vector3 originPos = t.position;

        t.localScale = Vector3.zero;
        Vector3 original = Vector3.one;

        while (t.localScale.x < 1)
        {
            t.localScale = Vector3.MoveTowards(t.localScale, original, 0.14f);
            yield return null;
        }
        t.localScale = original;
        Vector3 oPos = new Vector3(Random.Range(originPos.x, toPos.position.x), Random.Range(originPos.y, toPos.position.y-1));
        yield return new WaitForSecondsRealtime(0.02f);
        float timer = 0;

        float a = Random.Range(1.7f,2f);
        while (timer < 1)
        {
            t.position = CalculatePoint(timer, originPos, oPos,toPos.position);
            timer += Time.unscaledDeltaTime*a;
            a += Time.unscaledDeltaTime;
            yield return null;
        }
        t.position = toPos.position;

        Vector3 to = Vector3.zero;
        while (t.localScale.x >0)
        {
            t.localScale = Vector3.MoveTowards(t.localScale, to, 0.5f);
            yield return null;
        }
        t.gameObject.SetActive(false);

    }
}
public static class CanvasExtensions
{
    public static Vector2 WorldToCanvas(this Canvas canvas,
                                        Vector3 world_position,
                                        Camera camera = null)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        var viewport_position = camera.WorldToViewportPoint(world_position);
        var canvas_rect = canvas.GetComponent<RectTransform>();

        return new Vector2((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f),
                           (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f));
    }
}