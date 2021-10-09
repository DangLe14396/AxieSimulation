using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyEffect : MonoBehaviour
{
    public static FlyEffect Instance;
    [SerializeField]
    private Transform []holders;
    private GameObject  prefab;
    private List<List<Transform>> listObjTransform;
    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        listObjTransform = new List<List<Transform>>();
        for (int i = 0; i < this.holders.Length; i++)
        {
            Transform holder = this.holders[i];
            List<Transform> list = new List<Transform>();
            listObjTransform.Add(list);
            foreach (Transform t in holder)
            {
                list.Add(t);
            }
        }
    }

    int last = -1;
    public void SetUp(int index, Transform from, Transform to,System.Action callback,int count=1)
    {
        //if (last == index)
        //{
        //    Clean();
        //}
        //last = index;
        StartCoroutine(DoGenerate(index,count,from,to,callback));
    }
  
    public void Clean()
    {
        StopAllCoroutines();
        for (int i = 0; i < listObjTransform.Count; i++)
        {
            for (int j = 0; j < listObjTransform[i].Count; j++)
            {
                listObjTransform[i][j].gameObject.SetActive(false);
            }
        }
    }
    Transform Get(int i,int group)
    {
        try
        {
            if (listObjTransform[group][i].gameObject.activeSelf)
            {
                GameObject o = Instantiate(listObjTransform[group][0].gameObject, holders[group]);
                listObjTransform[group].Add(o.transform);
                return o.transform;
            }
            return listObjTransform[group][i];
        }
        catch
        {
            GameObject o = Instantiate(listObjTransform[group][0].gameObject, holders[group]);
            listObjTransform[group].Add(o.transform);
            return Get(i,group);

        }
    }
    IEnumerator DoGenerate(int group,int count,Transform from, Transform toPos, System.Action callback,float delay=0)
    {
        yield return new WaitForSecondsRealtime(delay);
     
        for(int i = 0; i < count; i++)
        {
            StartCoroutine(DoPopUp(group,i, from, toPos, callback));
            callback = null;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }


        isActive = false;

    }
    private Vector3 CalculatePoint(float t, Vector3 a, Vector3 b, Vector3 o)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * a;
        p += 2 * u * t * b;
        p += tt * o;
        return p;
    }
    IEnumerator DoPopUp( int group,int index,Transform from, Transform toPos, System.Action callback)
    {
        Transform t = Get(index,group);
        int i = 0;
        
        t.gameObject.SetActive(true);
        t.position = from.position ;
        Image[] imgs = t.GetComponentsInChildren<Image>();
        Color c = imgs[0].color;
        c.a = 1;
        imgs[0].color = c;


        t.localScale = Vector3.zero;
        Vector3 original = Vector3.one;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        while (t.localScale.x < 1)
        {
            t.position = from.position;
            t.localScale = Vector3.MoveTowards(t.localScale, original, 0.14f);
            yield return wait;
        }
        t.localScale = original;
        Vector3 originPos = t.position;
        Vector3 oPos = originPos+ (toPos.position-originPos).normalized*(toPos.position-originPos).magnitude/2f+Random.insideUnitSphere/2f;
        yield return new WaitForSecondsRealtime(0.02f);
        float timer = 0;

        float a = Random.Range(1f, 1.4f);
        while (timer < 1)
        {
            t.position = CalculatePoint(timer, originPos, oPos, toPos.position);
            timer += Time.unscaledDeltaTime * a;
            a += Time.unscaledDeltaTime;
            yield return wait;

        }
        t.position = toPos.position;
        callback?.Invoke();
        Vector3 to = new Vector3(2.43f,2.43f,1);
        c.a = 1;
        while (t.localScale.x <2.4f)
        {
            t.position = toPos.position;
            t.localScale = Vector3.Lerp(t.localScale, to, 0.25f);
            imgs[0].color = c;
            c.a = Mathf.Lerp(c.a, 0, 0.25f);
            yield return wait;

        }
        t.gameObject.SetActive(false);

    }
}
