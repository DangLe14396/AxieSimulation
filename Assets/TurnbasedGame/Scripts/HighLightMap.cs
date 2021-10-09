using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
public class HighLightMap : MonoBehaviour
{
    public static HighLightMap Instance;
    public HighLightTile[] tiles ;
    public Tilemap mainMap;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int range = 4;
    void Start()
    {
        Instance = this;
        int count = 1;
        for(int i = 1; i <= range; i++)
        {
            count += 6 * (i);
        }
        tiles = new HighLightTile[count];
        for(int i = 0; i < count; i++)
        {
            GameObject o = Instantiate(prefab, transform);
            HighLightTile tile = o.GetComponent<HighLightTile>();
            tiles[i]=(tile);
        }
    }
    public void Toggle()
    {
        isActive = !isActive;
        lastPos.x = -100000000;
        for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i].gameObject.SetActive(isActive);
        }
    }
    public bool isActive = false;
    Vector3Int lastPos;
    Vector3Int startPos ;
    Vector3 centerTilePos;
    public void OnUpdate(Vector3 pos)
    {
        if (mainMap == null || !isActive) return;
        Vector3Int cell = mainMap.WorldToCell(pos);
        float ratio = Vector3.Distance(pos, centerTilePos);
        //Debug.Log(ratio);
        if (lastPos != cell)
        {
            lastPos = cell;
            int index = 0;
            int range = this.range;
            Vector3Int center = HexGridHandler.OddrToCube(cell);
            centerTilePos = mainMap.CellToWorld(cell);
            startPos = center;
            tiles[index++].SetUp(mainMap.CellToWorld(HexGridHandler.CubeToOddr(startPos)), Color.grey);
            for (int r = 1; r <= range; r++)
            {
                startPos = new Vector3Int(center.x + HexGridHandler.directions[4].x * r, center.y + HexGridHandler.directions[4].y * r, center.z + HexGridHandler.directions[4].z * r);
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < r; j++)
                    {
                        tiles[index++].SetUp(mainMap.CellToWorld(HexGridHandler.CubeToOddr(startPos)), Color.white, r / 15f);
                        startPos = HexGridHandler.GetNeighbor(startPos, i, HexGridHandler.directions);
                      
                    }
                }
            }
        }
    }
    public void SetTile(Vector3 pos, Color color, float delayShow = 0)
    {
    }
    public void Hide()
    {
        isActive = false;
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].gameObject.SetActive(false);
        }
    }
  
}

