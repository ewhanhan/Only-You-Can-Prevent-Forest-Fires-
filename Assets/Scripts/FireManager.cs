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
    private float elapsedTime;
    private float startTime;

    public List<Vector3Int> fireSpots;
    public List<Vector3Int> currentFires;
    public TileBase fireTile;
    public TileBase treeBaseTile;
    public float fireTime;
    private GameMenuController instanceOfGameMenuController;

    public AudioSource fireSound;

    // Start is called before the first frame update
    void Start()
    {
        instanceOfGameMenuController = GameObject.Find("Canvas_UI").GetComponent<GameMenuController>();
        startTime = Time.time;

        int x_min = -8;
        int x_max = 8;
        int y_min = -9;
        int y_max = 8;

        for (int x = x_min; x <= x_max; x++)
        {
            for (int y = y_min; y <= y_max; y++)
            {
                Vector3Int localPlace = new Vector3Int(x, y, 0);
                if (objectTilemap.GetTile(localPlace) == treeBaseTile)
                {
                    fireSpots.Add(localPlace);
                }
            }
        }
    }

    void Update()
    {
        elapsedTime = Time.time - startTime;
        if (elapsedTime > fireTime && fireSpots.Count > 0)
        {
            startTime = Time.time;
            int randomSpot = Random.Range(0, fireSpots.Count);
            StartFire(fireSpots[randomSpot]);
            currentFires.Add(fireSpots[randomSpot]);
            fireSpots.RemoveAt(randomSpot);
        }
    }

    void StartFire(Vector3Int place)
    {   
        fireSound.Play();
        fireTilemap.SetTile(place, fireTile);
        // Vector3Int gridPosition = fireTilemap.WorldToCell(new Vector3Int(localPlace.x, localPlace.y + 1, 0));
        // GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        Vector3 fireSliderPosition = transform.TransformPoint(place);
        instanceOfGameMenuController.InstantiateFireHealth(fireSliderPosition);
    }
}
