using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private InputAction putOutFire;
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


    private void Awake()
    {
        playerMovement = new PlayerMovement();
        activeFires = 14;
    }

    private void OnEnable()
    {
        playerMovement.Enable();
    }

    private void DoPutOutFire(InputAction.CallbackContext obj)
    {
        // Debug.Log("Jump");
        Vector3Int gridPosition = fireTilemap.WorldToCell(transform.position);
        Vector3Int positionRight = new Vector3Int(gridPosition[0]+1, gridPosition[1], 0);
        Vector3Int positionLeft = new Vector3Int(gridPosition[0]-1, gridPosition[1], 0);
        Vector3Int positionUp = new Vector3Int(gridPosition[0], gridPosition[1]+1, 0);
        Vector3Int positionDown = new Vector3Int(gridPosition[0], gridPosition[1]-2, 0);

        if(fireTilemap.GetTile(positionRight)){
            fireTilemap.SetTile(positionRight, null);
            activeFires -= 1;
        }
        else if(fireTilemap.GetTile(positionLeft)){
            fireTilemap.SetTile(positionLeft, null);
            activeFires -= 1;
        }
        else if(fireTilemap.GetTile(positionDown)){
            fireTilemap.SetTile(positionDown, null);
            activeFires -= 1;
        }
        else if(fireTilemap.GetTile(positionUp)){
            fireTilemap.SetTile(positionUp, null);
            activeFires -= 1;
        }
    }

    private bool FireNearby()
    {
        return false;
    }

    private void OnDisable()
    {
        playerMovement.Disable();
    }

    void Start()
    {
        playerMovement.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        playerMovement.Main.PutOutFire.performed += DoPutOutFire;
    }

    private void Move(Vector2 direction)
    {
        if(CanMove(direction)){
            transform.position += (Vector3)direction;
            playerActive = true;
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if(!groundTilemap.HasTile(gridPosition) || objectTilemap.HasTile(gridPosition)){
            return false;
        }
        return true;
    }


    private bool NearFire()
    {
        return false;
    }
}
