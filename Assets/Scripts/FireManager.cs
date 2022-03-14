using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap objectTilemap;
    [SerializeField]
    private Tilemap fireTilemap;
    public List<Vector3Int> availablePlaces;
    public TileBase fireTile;
    public TileBase treeBaseTile;

    // Start is called before the first frame update
    void Start()
    {
        int x_min = -8;
        int x_max = 8;
        int y_min = -9;
        int y_max = 8;

        for(int x=x_min; x<=x_max; x++){
            for(int y=y_min; y<=y_max; y++){
                Vector3Int localPlace = new Vector3Int(x, y, 0);
                if (objectTilemap.GetTile(localPlace) == treeBaseTile)
                {
                    availablePlaces.Add(localPlace);
                    fireTilemap.SetTile(localPlace, fireTile);
                }
            }
        }
    }
}
