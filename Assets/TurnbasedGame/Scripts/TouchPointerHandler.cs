using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TouchPointerHandler : MonoBehaviour
{
    public delegate void OnClick(Vector3 mousePos);
    public static OnClick onClick;
  
    private void OnMouseDown()
    {
        //if (ActionPanel.Instance.gameObject.activeSelf) return;
        onClick?.Invoke(TouchHandler.GetMousePosition());

    }
}
