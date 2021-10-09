using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public static MiniMap Instance;
    Transform camTransform;

    [SerializeField]
    private RectTransform map;
    [SerializeField]
    private Transform markerHolder;
    private List<MiniMap_Mark> markers = new List<MiniMap_Mark>();
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        camTransform = CameraShake.Instance.GetTransfrom();

    }
    public void Register(CharacterController cc)
    {
        GameObject o = HighLightSpawner.Instance.Get(3);
        MiniMap_Mark marker = o.GetComponent<MiniMap_Mark>();
        marker.transform.SetParent(markerHolder);
        marker.transform.localScale = Vector3.one;
        marker.SetUp(cc);

    }
    // Update is called once per frame
    void Update()
    {
        Vector2 pos = -camTransform.localPosition;
        map.anchoredPosition = pos*20;
    }
}
