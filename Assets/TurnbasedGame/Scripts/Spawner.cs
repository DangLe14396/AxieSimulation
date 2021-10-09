using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    [SerializeField]
    private GameObject[] prefabs;
    [SerializeField]
    private List<List<GameObject>> stack = new List<List<GameObject>>();
    [SerializeField]
    int[] defaultCounts = { 10 };
    // Start is called before the first frame update
    void Start()
    {
        //Init();
    }
    public virtual void Init()
    {
        for (int j = 0; j < prefabs.Length; j++)
        {
            stack.Add(new List<GameObject>());
            if (prefabs[j] == null) continue;
            for (int i = 0; i < defaultCounts[j]; i++)
            {
                GameObject o = Instantiate(prefabs[j], transform);
                o.name += " " + stack[j].Count;

                stack[j].Add(o);
                o.SetActive(false);
            }
        }
        //StartCoroutine(DoInit());
    }
    IEnumerator DoInit()
    {
       
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        for (int j = 0; j < prefabs.Length; j++)
        {
            stack.Add(new List<GameObject>());
            for (int i = 0; i < defaultCounts[j]; i++)
            {
                GameObject o = Instantiate(prefabs[j], transform);
                o.name += " " + stack[j].Count;
                stack[j].Add(o);
                o.SetActive(false);
                yield return wait;
                yield return wait;
                yield return wait;
                yield return wait;
                yield return wait;
                yield return wait;
            }
        }
    }
    public virtual void ClearAll()
    {
        for (int i = 0; i < stack.Count; i++)
        {
            for (int j = 0; j < stack[i].Count; j++)
            {
                stack[i][j].SetActive(false);
            }
        }
    }
    public GameObject GetAt(int index = 0, int type = 0)
    {
        if (stack[type].Count <= index)
        {
            GameObject o = Instantiate(prefabs[type], transform);
            stack[type].Add(o);
            o.SetActive(false);
            return o;
        }
        return stack[type][index];
    }
    public GameObject Get(int type = -1)
    {
        type = type == -1 ? Random.Range(0, stack.Count) : type;
        for (int i = 0; i < stack[type].Count; i++)
        {
            if (!stack[type][i].activeSelf)
            {
                return stack[type][i];
            }
        }
        GameObject o = Instantiate(prefabs[type], transform);
        o.name += " " + stack[type].Count;
        stack[type].Add(o);
        o.SetActive(false);
        return o;
    }
}
