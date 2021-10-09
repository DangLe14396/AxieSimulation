using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinSparkEffect : EffectAbstract
{
    [SerializeField]
    private ParticleSystem ps;
    [SerializeField]
    private ParticleSystem[] otherPS;
    [SerializeField]
    private TextMeshPro text;
    Transform _transform;
    public override void Active(Vector3 pos, Color color)
    {
        throw new System.NotImplementedException();
    }
    public override void Active(Transform parent)
    {
        transform.parent = parent;
        gameObject.SetActive(true);
        ps.Play();
        //StartCoroutine(DoWait(parent.gameObject));
    }
    public override void Active(Vector3 pos, string amount)
    {
        if (_transform == null)
        {
            _transform = transform;
        }

        isUsing = true;
        transform.position = pos;
        gameObject.SetActive(true);
        ps.Play();
        if (text != null)
        {
            text.text = amount;
            StopAllCoroutines();
            StartCoroutine(DoShowCoinPS());
        }
    }
    public override void Active(Vector3 pos,int amount)
    {
        if (_transform == null)
        {
            _transform = transform;
        }

        isUsing = true;
        transform.position = pos;
        gameObject.SetActive(true);
        ps.Play();
        if (text != null)
        {
            text.text = (amount<0?"":"+")+amount;
            StopAllCoroutines();
            StartCoroutine(DoShowCoinPS());
        }
    }
    public override void Active(Vector3 pos)
    {
        if (_transform == null)
        {
            _transform = transform;
        }

        isUsing = true;
        transform.position = pos;
        gameObject.SetActive(true);
        ps.Play();
        if (text != null)
        {
            StopAllCoroutines();
            StartCoroutine(DoShowCoinPS());
        }
    }
    Color c;
    IEnumerator DoShowCoinPS()
    {
        c = text.color;
        c.a = 0;
        text.color = c;
        Transform t = text.transform;
        t.localScale = Vector3.one;
        Vector3 scale = t.localScale;
        t.localScale = new Vector3(0, 0, 1);
        Vector3 move = new Vector3(0, 0.5f*Time.deltaTime);
        while (c.a < 0.98f)
        {
            c.a = Mathf.Lerp(c.a, 1, 0.25f);
            t.localScale = Vector3.Lerp(t.localScale, scale, 0.25f);
            text.color = c;
            yield return null;
        }
        float a = 0;
        float s = t.localScale.x;
        while (a < Mathf.PI)
        {
            t.localScale = Vector3.Lerp(t.localScale, scale, 0.25f);
            scale.x = scale.y = s + Mathf.Sin(a) / 10f;
            a += Mathf.PI / 45;
            _transform.localPosition += move;
            yield return null;
        }
        scale.x = scale.y = 1.5f;
        while (c.a > 0.02f)
        {
            c.a = Mathf.Lerp(c.a, 0, 0.2f);
            text.color = c;
            t.localScale = Vector3.Lerp(t.localScale, scale, 0.25f);

            yield return null;
        }
        c.a = 0;
        text.color = c;
        isUsing = false;
    }

    public override void Active(Vector3 pos, Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public override bool IsUsing()
    {
        return isUsing;
    }
    public override void Active(SpriteRenderer sr)
    {
        Debug.Log(sr.gameObject.name);
        gameObject.SetActive(true);
        ParticleSystem.ShapeModule shape = ps.shape;
        shape.spriteRenderer = sr;
        for (int i = 0; i < otherPS.Length; i++)
        {
            ParticleSystem.ShapeModule s = otherPS[i].shape;
            s.spriteRenderer = sr;
        }
        ps.Play();
    }
}
