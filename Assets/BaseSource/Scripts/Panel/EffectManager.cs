using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    [SerializeField]
    private GameObject[] prefabs;
    [SerializeField]
    private List<List<EffectAbstract>> list=new List<List<EffectAbstract>>();
    [SerializeField]
    private ParticleSystem[] fireworks,fireworks2;
    [SerializeField]
    private Transform firework;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Init();
    }
    float a = 0;
    float sinValue = 0;
    private void Update()
    {
        sinValue = Mathf.Sin(a);
        a += Mathf.PI / 20f;
        if (a >= Mathf.PI * 2)
        {
            a = 0;
        }
    }
    public float GetSinValue()
    {
        return sinValue;
    }
    public void PlayFireWork2()
    {
        SoundManager.Instance.PlaySFX(SFX.firework, 1f);
        for (int i = 0; i < fireworks2.Length; i++)
        {
            fireworks2[i].Play();
        }
    }
    public void PlayFireWork()
    {
        for(int i = 0; i < fireworks.Length; i++)
        {
            fireworks[i].Play();
        }
    }
    public void Init()
    {
        Transform holder = transform;
        for(int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] == null) continue;
            int total = 1;
            List<EffectAbstract> l = new List<EffectAbstract>();
            list.Add(l);
            for(int j = 0; j < total; j++)
            {
                GameObject o = Instantiate(prefabs[i], holder);
                EffectAbstract ea = o.GetComponent<EffectAbstract>();
                l.Add(ea);
                ea.Deactive();
            }
        }
        System.GC.Collect();
    }
    public EffectAbstract Get(int type)
    {
        List<EffectAbstract> l = list[type];
        for(int i = 0; i < l.Count; i++)
        {
            if (!l[i].IsUsing())
            {
                return l[i];
            }
        }
        GameObject o = Instantiate(prefabs[type], transform);
        EffectAbstract ea = o.GetComponent<EffectAbstract>();
        l.Add(ea);
        ea.Deactive();

        return ea;

    }
}

public abstract class EffectAbstract : MonoBehaviour
{
    public bool isUsing = false;
    public virtual void Active() { }
    public virtual void Active(Transform parent) { }
    public virtual void Active(SpriteRenderer sr) { }
    public abstract void Active(Vector3 pos, Color color);
    public abstract void Active(Vector3 pos);
    public abstract void Active(Vector3 pos,Vector3 direction);
    public virtual void Active(Vector3 pos, int amount) { }
    public virtual void Active(Vector3 pos, int amount,bool isCritical) { }
    public virtual void Active(Vector3 pos, string text) { }
    public abstract bool IsUsing();
    public virtual void Deactive()
    {
        gameObject.SetActive(false);
        isUsing = false;
    }
}