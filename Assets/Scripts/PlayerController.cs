using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public FireManager fireManager;
    public CamperManager camperManager;
    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap objectTilemap;
    [SerializeField]
    private Tilemap fireTilemap;
    public TileBase fireTile;
    [HideInInspector]
    public bool playerActive;
    [HideInInspector]
    public int bucketWaterQuantity;
    public TileBase camperTile;
    public TileBase wellTile;

    public AudioSource footStep;
    public AudioSource waterBucket;
    public AudioSource emptyBucket;
    public AudioSource hushCamper;
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask stopMovement;
    public Text bucketText;
    public Text currentFiresText;
    public Text loudCampers;


    private void Awake()
    {
        playerActive = true;
    }

    void Start()
    {
        movePoint.parent = null;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (CanMove(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f)))
                {
                    footStep.Play();
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (CanMove(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f)))
                {
                    footStep.Play();
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
            }
        }

        if (Input.GetKeyDown("space"))
        {
            DoPutOutFire();
            DoQuietCamper();
            RefillBucket();
        }

    }

    private void RefillBucket()
    {
        Vector3Int gridPosition = objectTilemap.WorldToCell(movePoint.position);
        Vector3Int positionRight = new Vector3Int(gridPosition[0] + 1, gridPosition[1], 0);
        Vector3Int positionLeft = new Vector3Int(gridPosition[0] - 1, gridPosition[1], 0);
        Vector3Int positionUp = new Vector3Int(gridPosition[0], gridPosition[1] + 1, 0);
        Vector3Int positionDown = new Vector3Int(gridPosition[0], gridPosition[1] - 1, 0);

        if (objectTilemap.GetTile(positionRight) == wellTile || objectTilemap.GetTile(positionLeft) == wellTile ||
            objectTilemap.GetTile(positionUp) == wellTile || objectTilemap.GetTile(positionDown) == wellTile)
        {
            waterBucket.Play();
            bucketWaterQuantity = 10;
            bucketText.text = "BUCKET - " + bucketWaterQuantity.ToString();
        }
    }

    private void DoQuietCamper()
    {
        Vector3Int gridPosition = objectTilemap.WorldToCell(movePoint.position);
        Vector3Int positionRight = new Vector3Int(gridPosition[0] + 1, gridPosition[1], 0);
        Vector3Int positionLeft = new Vector3Int(gridPosition[0] - 1, gridPosition[1], 0);
        Vector3Int positionUp = new Vector3Int(gridPosition[0], gridPosition[1] + 1, 0);
        Vector3Int positionDown = new Vector3Int(gridPosition[0], gridPosition[1] - 1, 0);

        if (objectTilemap.GetTile(positionRight) == camperTile)
        {
            objectTilemap.SetTile(positionRight, null);
            hushCamper.Play();
            camperManager.camperSpots.Add(positionRight);
            camperManager.currentCampers.Remove(positionRight);
            loudCampers.text = "LOUD CAMPERS - " + camperManager.currentCampers.Count.ToString();
        }
        if (objectTilemap.GetTile(positionLeft) == camperTile)
        {
            objectTilemap.SetTile(positionLeft, null);
            hushCamper.Play();
            camperManager.camperSpots.Add(positionLeft);
            camperManager.currentCampers.Remove(positionLeft);
            loudCampers.text = "LOUD CAMPERS - " + camperManager.currentCampers.Count.ToString();
        }
        if (objectTilemap.GetTile(positionDown) == camperTile)
        {
            objectTilemap.SetTile(positionDown, null);
            hushCamper.Play();
            camperManager.camperSpots.Add(positionDown);
            camperManager.currentCampers.Remove(positionDown);
            loudCampers.text = "LOUD CAMPERS - " + camperManager.currentCampers.Count.ToString();
        }
        if (objectTilemap.GetTile(positionUp) == camperTile)
        {
            objectTilemap.SetTile(positionUp, null);
            hushCamper.Play();
            camperManager.camperSpots.Add(positionUp);
            camperManager.currentCampers.Remove(positionUp);
            loudCampers.text = "LOUD CAMPERS - " + camperManager.currentCampers.Count.ToString();
        }
    }

    private void DoPutOutFire()
    {
        Vector3Int gridPosition = fireTilemap.WorldToCell(movePoint.position);
        Vector3Int positionRight = new Vector3Int(gridPosition[0]+1, gridPosition[1], 0);
        Vector3Int positionLeft = new Vector3Int(gridPosition[0]-1, gridPosition[1], 0);
        Vector3Int positionUp = new Vector3Int(gridPosition[0], gridPosition[1]+1, 0);
        Vector3Int positionDown = new Vector3Int(gridPosition[0], gridPosition[1]-2, 0);

        if(bucketWaterQuantity > 0)
        {
            if(fireTilemap.GetTile(positionRight)){
                fireTilemap.SetTile(positionRight, null);
                waterBucket.Play();
                bucketWaterQuantity -= 1;
                fireManager.fireSpots.Add(positionRight);
                fireManager.currentFires.Remove(positionRight);
                bucketText.text = "BUCKET - " + bucketWaterQuantity.ToString();
                currentFiresText.text = "FIRES - " + fireManager.currentFires.Count.ToString();
            }
            if (fireTilemap.GetTile(positionLeft))
            {
                fireTilemap.SetTile(positionLeft, null);
                waterBucket.Play();
                bucketWaterQuantity -= 1;
                fireManager.fireSpots.Add(positionLeft);
                fireManager.currentFires.Remove(positionLeft);
                bucketText.text = "BUCKET - " + bucketWaterQuantity.ToString();
                currentFiresText.text = "FIRES - " + fireManager.currentFires.Count.ToString();
            }
            if (fireTilemap.GetTile(positionDown))
            {
                fireTilemap.SetTile(positionDown, null);
                waterBucket.Play();
                bucketWaterQuantity -= 1;
                fireManager.fireSpots.Add(positionDown);
                fireManager.currentFires.Remove(positionDown);
                bucketText.text = "BUCKET - " + bucketWaterQuantity.ToString();
                currentFiresText.text = "FIRES - " + fireManager.currentFires.Count.ToString();
            }
            if (fireTilemap.GetTile(positionUp))
            {
                fireTilemap.SetTile(positionUp, null);
                waterBucket.Play();
                bucketWaterQuantity -= 1;
                fireManager.fireSpots.Add(positionUp);
                fireManager.currentFires.Remove(positionUp);
                bucketText.text = "BUCKET - " + bucketWaterQuantity.ToString();
                currentFiresText.text = "FIRES - " + fireManager.currentFires.Count.ToString();
            }
        } else if (bucketWaterQuantity <= 0 && (fireTilemap.GetTile(positionUp) || fireTilemap.GetTile(positionDown) || fireTilemap.GetTile(positionRight) || fireTilemap.GetTile(positionLeft))){
            
            emptyBucket.Play();
        }
    }

    private bool CanMove(Vector3 direction)
    {
        Vector3Int gridPosition = objectTilemap.WorldToCell(direction);
        if (!groundTilemap.HasTile(gridPosition) || objectTilemap.HasTile(gridPosition))
        {
            return false;
        }
        return true;
    }
}
