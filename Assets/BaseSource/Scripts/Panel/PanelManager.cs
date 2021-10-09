using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;
    [SerializeField]
    private List<Panel> panels = new List<Panel>();
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        Instance = this;
        Transform holder = transform;
        for(int i = 0; i < panels.Count; i++)
        {
            panels[i].Init();
        }
        for(int i = 0; i < holder.childCount; i++)
        {
            Panel panel = holder.GetChild(i).GetComponent<Panel>();
            if (panel == null) continue;
            try
            {
                panel.Init();
                panels.Add(panel);
            }
            catch (System.Exception e){ Debug.LogError(panel.gameObject.name+" \n"+e); }
        }
    }
    public Panel GetPanel(int index)
    {
        return panels[index];
    }
}
