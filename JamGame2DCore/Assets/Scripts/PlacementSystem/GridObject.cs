using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridObject : MonoBehaviour
{
    [SerializeField] protected Grid grid;

    // Track the grid cells this object occupies
    [SerializeField] private Vector3Int[] occupiedCells;

    protected Collider2D objectCollider;

    protected virtual void Awake()
    {
        objectCollider = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        UpdateOccupiedCells();
    }

    protected virtual void Update()
    {
        // UpdateOccupiedCells();
    }

    // public void UpdateOccupiedCells()
    // {
    //     List<Vector3Int> occupiedCellsList = new List<Vector3Int>();

    //     // Assuming that the object's children represent the shape's individual cells
    //     foreach (Transform child in transform)
    //     {
    //         Vector3Int gridPosition = grid.WorldToCell(child.position);
    //         occupiedCellsList.Add(gridPosition);
    //         Debug.Log($"Added {gridPosition} to the list");
    //     }

    //     occupiedCells = occupiedCellsList.ToArray();
    // }

    public void UpdateOccupiedCells()
    {
        // Debug.Log($"Updating cells from {transform.position}");
        List<Vector3Int> occupiedPositions = new List<Vector3Int>();

        if (objectCollider is BoxCollider2D)
        {
            // Handle BoxCollider2D
            occupiedPositions = GetOccupiedPositionsForBoxCollider(objectCollider as BoxCollider2D);
        }
        else if (objectCollider is PolygonCollider2D)
        {
            // Handle PolygonCollider2D
            occupiedPositions = GetOccupiedPositionsForPolygonCollider(objectCollider as PolygonCollider2D);
        }

        occupiedCells = occupiedPositions.ToArray();
    }

    private List<Vector3Int> GetOccupiedPositionsForBoxCollider(BoxCollider2D boxCollider)
    {
        List<Vector3Int> occupiedPositions = new List<Vector3Int>();

        Bounds bounds = boxCollider.bounds;
        bounds.extents -= new Vector3(0.2f, 0.2f, 0.0f);
        // Debug.Log($"collider bounds {boxCollider.bounds}");
        Vector3Int minGridPos = grid.WorldToCell(bounds.min);
        Vector3Int maxGridPos = grid.WorldToCell(bounds.max);

        for (int x = minGridPos.x; x <= maxGridPos.x; x++)
        {
            for (int y = minGridPos.y; y <= maxGridPos.y; y++)
            {
                occupiedPositions.Add(new Vector3Int(x, y));
            }
        }

        return occupiedPositions;
    }

    private List<Vector3Int> GetOccupiedPositionsForPolygonCollider(PolygonCollider2D polygonCollider)
    {
        List<Vector3Int> occupiedPositions = new List<Vector3Int>();

        Bounds bounds = polygonCollider.bounds;
        Vector3Int minGridPos = grid.WorldToCell(bounds.min);
        Vector3Int maxGridPos = grid.WorldToCell(bounds.max);

        for (int x = minGridPos.x; x <= maxGridPos.x; x++)
        {
            for (int y = minGridPos.y; y <= maxGridPos.y; y++)
            {
                // Debug.Log($"Checking Cell {x}, {y}");
                Vector3 cellCornerPos = grid.CellToWorld(new Vector3Int(x, y));
                // Apply slight offset for cellCenter
                cellCornerPos += new Vector3(0.2f, 0.2f, 0.0f);
                // Debug.Log($"{grid.CellToWorld(new Vector3Int(x, y))} and {polygonCollider.OverlapPoint(cellCornerPos)}");

                if (polygonCollider.OverlapPoint(cellCornerPos))
                {
                    occupiedPositions.Add(new Vector3Int(x, y, 0));
                }
            }
        }

        return occupiedPositions;
    }

    public bool ContainsCell(Vector3Int cellPosition)
    {
        return occupiedCells.Contains(cellPosition);
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

    protected bool IsCellValid(Vector3Int cellPosition)
    {
        // Check if the cell is occupied by another object
        foreach (GameObject boardObject in FindObjectOfType<GridManager>().objectsOnGrid)
        {
            var gridObject = boardObject.GetComponent<GridObject>();
            if (boardObject != gameObject && gridObject != null && gridObject.occupiedCells.Contains(cellPosition))
            {
                return false;
            }
        }
        // Check if cell is within play area
        

        return true;
    }
}
