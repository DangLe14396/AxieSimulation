using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCoin : MonoBehaviour
{
    Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float t = 0;
    Vector3 from, to,mid;
    System.Action callback;
    public void SetUp(Vector3 from,Vector3 to,System.Action callback)
    {
        if (_transform == null)
        {
            _transform = transform;
        }
        this.callback = callback;
        this.from = from;
        this.to = to;
        mid = new Vector3(Random.Range(from.x, to.x), Random.Range(from.y, to.y - 1));
        t = 0;
        rot.z = 50;
        scale.x = scale.y = scale.z = 0;
        a = 0;
        _transform.localScale = scale;
        gameObject.SetActive(true);
    }
    Vector3 scale = Vector3.one;
    Vector3 rot = new Vector3(0, 0, 100);
    float a = 0;
    private void Update()
    {
        if (t < 0.99)
        {
            scale.x = scale.y = scale.z = Mathf.Lerp(scale.x, 1, 0.2f);
            _transform.localScale = scale;
            //_transform.Rotate(rot);
            rot.z = Mathf.Lerp(rot.z, 5, 0.2f);
            _transform.localPosition = CalculatePoint(t, from, mid, to);
            t = Mathf.Lerp(t, 1, a);
            a += Time.deltaTime / 6f;
            a = a > 0.1 ? 0.1f : a;
        }
        else
        {
            scale.x = scale.y = scale.z = Mathf.Lerp(scale.x, 0, 0.2f);
            _transform.localScale=scale;
        }
        if (t >=0.99)
        {
            callback?.Invoke();
            gameObject.SetActive(false);
        }

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
}
