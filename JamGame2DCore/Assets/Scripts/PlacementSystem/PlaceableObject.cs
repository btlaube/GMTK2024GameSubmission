using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  // Added for Linq functions

public class PlaceableObject : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    private bool isPlacingObject = false;
    private Vector3 initialPosition;
    private Camera m_Camera;

    // Track the grid cells this object occupies
    public Vector3Int[] occupiedCells;

    void Awake()
    {
        m_Camera = Camera.main;
    }

    void Start()
    {
        UpdateOccupiedCells();
    }

    void Update()
    {
        if (isPlacingObject)
        {
            UpdateObjectPosition();
            UpdateOccupiedCells();
        }
    }

    // private void OnMouseDown()
    // {
    //     if (!isPlacingObject)
    //     {
    //         StartPlacingObject();
    //     }
    // }

    public void StartPlacingObject()
    {
        isPlacingObject = true;
        initialPosition = transform.position;

        // Call to hold the object in the player's inventory
        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
        playerInventory.HoldMovableObject(gameObject);

        UpdateOccupiedCells();
    }

    public void StopPlacingObject()
    {
        isPlacingObject = false;
    }

    private void UpdateObjectPosition()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPositionMouse();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        transform.position = grid.CellToWorld(gridPosition);
    }

    public void UpdateOccupiedCells()
    {
        List<Vector3Int> occupiedCellsList = new List<Vector3Int>();

        // Assuming that the object's children represent the shape's individual cells
        foreach (Transform child in transform)
        {
            Vector3Int gridPosition = grid.WorldToCell(child.position);
            occupiedCellsList.Add(gridPosition);
            Debug.Log($"Added {gridPosition} to the list");
        }

        occupiedCells = occupiedCellsList.ToArray();
    }

    public bool IsPositionValid(Vector3Int newPosition)
    {
        foreach (Vector3Int cell in occupiedCells)
        {
            Vector3Int potentialPosition = newPosition + (cell - grid.WorldToCell(transform.position));
            if (!IsCellValid(potentialPosition))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsCellValid(Vector3Int cellPosition)
    {
        // Assuming the grid is infinite or you have predefined bounds, you might need to check against them.
        // Example: if you have bounds, check them here (e.g., if (cellPosition.x < minX || cellPosition.x > maxX)...)

        // Check if the cell is occupied by another object
        foreach (GameObject boardObject in FindObjectOfType<GridManager>().objectsOnGrid)
        {
            var placeableObject = boardObject.GetComponent<PlaceableObject>();
            if (boardObject != gameObject && placeableObject != null && placeableObject.occupiedCells.Contains(cellPosition))
            {
                return false;
            }
        }
        return true;
    }

    public bool ContainsCell(Vector3Int cellPosition)
    {
        return occupiedCells.Contains(cellPosition);
    }
}
