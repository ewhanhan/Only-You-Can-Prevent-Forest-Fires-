using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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
    public Text loudCampers;
    public List<Vector3Int> allAvailableCamperSpots;
    private GameMenuController instanceOfGameMenuController;
    public AudioSource campSound;
    public AudioSource humanSound;


    // Start is called before the first frame update
    void Start()
    {
        instanceOfGameMenuController = GameObject.Find("Canvas_UI").GetComponent<GameMenuController>();
        startTime = Time.time;

        camperSpots.Add(new Vector3Int(-14, -8, 0));
        allAvailableCamperSpots.Add(new Vector3Int(-14, -8, 0));
        camperSpots.Add(new Vector3Int(-11, 6, 0));
        allAvailableCamperSpots.Add(new Vector3Int(-11, 6, 0));
        camperSpots.Add(new Vector3Int(-7, 6, 0));
        allAvailableCamperSpots.Add(new Vector3Int(-7, 6, 0));        
        camperSpots.Add(new Vector3Int(9, 6, 0));
        allAvailableCamperSpots.Add(new Vector3Int(9, 6, 0));
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime = Time.time - startTime;
        if (elapsedTime > camperTime && camperSpots.Count > 0)
        {
            startTime = Time.time;
            int randomSpot = Random.Range(0, camperSpots.Count);
            AddCamper(camperSpots[randomSpot]);
            campSound.Play();
            humanSound.Play();
            currentCampers.Add(camperSpots[randomSpot]);
            camperSpots.RemoveAt(randomSpot);
            loudCampers.text = "LOUD CAMPERS - " + currentCampers.Count.ToString();
        }
    }

    void AddCamper(Vector3Int place)
    {
        objectTilemap.SetTile(place, camperTile);
        instanceOfGameMenuController.InstantiateCamperHealth(place);
    }
}
