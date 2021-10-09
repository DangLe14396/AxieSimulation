using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTut : MonoBehaviour
{
    Transform t;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void SetUp(Vector3 from,Vector3 to)
    {
        t = transform;
        t.position = from;
        gameObject.SetActive(true);
        StartCoroutine(DoMove(from, to));
    }
    IEnumerator DoMove(Vector3 from,Vector3 to)
    {
        while (true)
        {
            t.localPosition = from;
            yield return new WaitForSeconds(0.6f);

            while (Vector3.Distance(t.localPosition, to) > 0.02f)
            {
                t.localPosition = Vector3.MoveTowards(t.localPosition, to, 5*Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(0.4f);
        }
    }
    public void SetUp(Vector3 pos)
    {
        pos.y = 0.35f;
        transform.position = pos;
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
