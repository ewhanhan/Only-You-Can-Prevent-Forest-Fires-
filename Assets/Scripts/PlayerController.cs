using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public FireManager fireManager;
    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap objectTilemap;
    [SerializeField]
    private Tilemap fireTilemap;
    public TileBase fireTile;
    [HideInInspector] 
    public int activeFires;
    [HideInInspector] 
    public bool playerActive;

    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask stopMovement;
    public AudioSource footStep;
    public AudioSource waterBucket;

    private void Awake()
    {
        activeFires = 14;
    }

    void Start()
    {
        movePoint.parent = null;
    }
    
    void Update() 
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(CanMove(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f)))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }

            if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if(CanMove(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f)))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
            }
        }

        if(Input.GetKeyDown("space"))
        {
            DoPutOutFire();
        }
    }

    private void DoPutOutFire()
    {
        Vector3Int gridPosition = fireTilemap.WorldToCell(movePoint.position);
        Vector3Int positionRight = new Vector3Int(gridPosition[0]+1, gridPosition[1], 0);
        Vector3Int positionLeft = new Vector3Int(gridPosition[0]-1, gridPosition[1], 0);
        Vector3Int positionUp = new Vector3Int(gridPosition[0], gridPosition[1]+1, 0);
        Vector3Int positionDown = new Vector3Int(gridPosition[0], gridPosition[1]-2, 0);

        if(fireTilemap.GetTile(positionRight)){
            fireTilemap.SetTile(positionRight, null);
            fireManager.fireSpots.Add(positionRight);
            waterBucket.Play();
            activeFires -= 1;
        }
        if(fireTilemap.GetTile(positionLeft)){
            fireTilemap.SetTile(positionLeft, null);
            fireManager.fireSpots.Add(positionLeft);
            waterBucket.Play();
            activeFires -= 1;
        }
        if(fireTilemap.GetTile(positionDown)){
            fireTilemap.SetTile(positionDown, null);
            fireManager.fireSpots.Add(positionDown);
            waterBucket.Play();
            activeFires -= 1;
        }
        if(fireTilemap.GetTile(positionUp)){
            fireTilemap.SetTile(positionUp, null);
            fireManager.fireSpots.Add(positionUp);
            waterBucket.Play();
            activeFires -= 1;
        }
    }

    private bool CanMove(Vector3 direction)
    {
        Vector3Int gridPosition = objectTilemap.WorldToCell(direction);
        if(!groundTilemap.HasTile(gridPosition) || objectTilemap.HasTile(gridPosition)){
            return false;
        }
        return true;
    }
}
