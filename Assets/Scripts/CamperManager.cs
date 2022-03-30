using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CamperManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap objectTilemap;
    public List<Vector3Int> camperSpots;
    public List<Vector3Int> currentCampers;
    public TileBase camperTile;
    private float elapsedTime;
    private float startTime;
    public float camperTime;

    public int x;
    public int y;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

        camperSpots.Add(new Vector3Int(-14, -8, 0));
        camperSpots.Add(new Vector3Int(-7, 6, 0));
        camperSpots.Add(new Vector3Int(-11, 6, 0));
        camperSpots.Add(new Vector3Int(9, 6, 0));
    
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime = Time.time - startTime;
        if(elapsedTime > camperTime && camperSpots.Count > 0){
            startTime = Time.time;
            int randomSpot = Random.Range(0, camperSpots.Count);
            AddCamper(camperSpots[randomSpot]);
            currentCampers.Add(camperSpots[randomSpot]);
            camperSpots.RemoveAt(randomSpot);
        }
    }

    void AddCamper(Vector3Int place)
    {
        objectTilemap.SetTile(place, camperTile);
    }
}
