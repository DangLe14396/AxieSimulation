using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextEffect : EffectAbstract
{
    [SerializeField]
    private ParticleSystem ps;
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
 
    [SerializeField]
    private Color critColor, normalColor,missedColor;
    Color toColor;
    public override void Active(Vector3 pos, int amount,bool isCritical)
    {
        if (_transform == null)
        {
            _transform = transform;
        }


        isUsing = true;
        transform.position = pos;
        gameObject.SetActive(true);
        if (isCritical)
        {
            ps.Play();
        }
        if (text != null)
        {
            text.text =  amount.ToString();
            text.color = Color.white;
            toColor = isCritical ? critColor : normalColor;
            text.fontSize = isCritical?4f:2.5f;
            StopAllCoroutines();
            StartCoroutine(DoShowCoinPS(isCritical));
        }
    }
    public override void Active(Vector3 pos, string txt)
    {
        if (_transform == null)
        {
            _transform = transform;
        }


        isUsing = true;
        transform.position = pos;
        gameObject.SetActive(true);
       
        if (text != null)
        {
            text.text = txt.ToString();
            text.color = Color.white;
            toColor = missedColor;
            text.fontSize =  2.5f;
            StopAllCoroutines();
            StartCoroutine(DoShowCoinPS(false));
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
            StartCoroutine(DoShowCoinPS(false));
        }
    }
    Color c;
    IEnumerator DoShowCoinPS(bool isCritical)
    {
        c = text.color;
        c.a = 0;
        text.color = c;
        Transform t = text.transform;
        t.localScale = Vector3.one;
        Vector3 scale = t.localScale;
        t.localScale = new Vector3(0, 0, 1);
        Vector3 move = new Vector3(0, (isCritical ? 0.22f: 0.35f) * Time.deltaTime);
        Vector3 offset = (Vector3)Random.insideUnitCircle / 2f;
        offset.y = offset.y < 0 ? 0 : offset.y;
        Vector3 newPos = transform.position + offset;
        scale.x = scale.y = 1.3f;
        while (c.a < 0.98f)
        {
            c = Color.Lerp(c, toColor, 0.25f);
            t.localScale = Vector3.Lerp(t.localScale, scale, 0.25f);
            t.position = Vector3.Lerp(t.position, newPos+(Vector3)Random.insideUnitCircle/20f, 0.25f);
            text.color = c;
            yield return null;
        }
        float a = 0;
        float s = t.localScale.x;
        scale.x = scale.y = 1;
        while (a < Mathf.PI*(isCritical?3:1))
        {
            //text.color = c;
            //c = Color.Lerp(c, toColor, 0.3f);


            t.localScale = Vector3.Lerp(t.localScale, scale, 0.25f);
            scale.x = scale.y = s + Mathf.Sin(a) / 10f;
            a += Mathf.PI / (isCritical?30:45);
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
       
        ps.Play();
    }
}
