using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MainTileMap : MonoBehaviour
{
    public Tilemap mainMap;
    [SerializeField]
    private TileBase blurTileBase;
    // Start is called before the first frame update
    void Start()
    {
        TouchPointerHandler.onClick -= OnClick;
        TouchPointerHandler.onClick += OnClick;
    }
    private void OnDisable()
    {
        TouchPointerHandler.onClick -= OnClick;
    }
    public Vector3Int WorldToCell(Vector3 pos)
    {
        return mainMap.WorldToCell(pos);
    }
    public void OnClick(Vector3 mousePos)
    {
        Vector3Int cellPos = mainMap.WorldToCell(mousePos);
        GameManager.Instance.OnCellSelected(cellPos);
    }
    Vector3Int lastPos=Vector3Int.one;
    public void HighLightTile(Vector3 cameraPos)
    {
        //Vector3Int center = mainMap.WorldToCell(cameraPos);
        //if (center != lastPos)
        //{
        //    lastPos = center;
        //    mainMap.RefreshAllTiles();
        //    center = HexGridHandler.OddrToCube(center);
        //    int range = 4;
        //    Vector3Int startPos = center;
        //    mainMap.SetTile(HexGridHandler.CubeToOddr(startPos), blurTileBase);
        //    for (int r = 1; r <= range; r++)
        //    {
        //        startPos = new Vector3Int(center.x + HexGridHandler.directions[4].x * r, center.y + HexGridHandler.directions[4].y * r, center.z + HexGridHandler.directions[4].z * r);
        //        for (int i = 0; i < 6; i++)
        //        {
        //            for (int j = 0; j < r; j++)
        //            {
        //                mainMap.SetTile(HexGridHandler.CubeToOddr(startPos),blurTileBase);

        //                startPos = HexGridHandler.GetNeighbor(startPos, i, HexGridHandler.directions);
        //            }
        //        }
        //    }
        //}
    }
    public Vector3 CellToWorld(Vector3Int pos)
    {
        return mainMap.CellToWorld(pos);
    }


}
