using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCheckHandler : MonoBehaviour
{
    [SerializeField] protected Grid grid;

    [SerializeField] private Collider2D objectCollider;

    // The serialized array of Transforms representing the correct order of the solution
    // [SerializeField] private Transform[] correctSolution;

    // For after checking puzzle, load level select (correct)
    private LevelLoader levelLoader;

    // List of occupied cells
    [SerializeField] private Vector3Int[] occupiedCells;


    void Start()
    {
        levelLoader = LevelLoader.instance;

        UpdateOccupiedCells();
    }

    // void Update()
    // {
    //     // Check if the puzzle is solved
    //     if (CheckSolution())
    //     {
    //         Debug.Log("Puzzle solved!");
    //         // Add any additional logic for a solved puzzle here
    //     }
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
        // Collider offset
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

    public void SubmitSolution()
    {
        if (CheckSolution())
        {
            Debug.Log("asdf");
            // Handle solved puzzle submitted
            levelLoader.LoadScene(1);

        }
        else
        {
            // Handle incorrect solution submitted

        }
    }

    public bool CheckSolution()
    {
        // Check that all placeable objects have the correct position and scale
            //  Determined by the solution
        foreach (GameObject boardObject in FindObjectOfType<GridManager>().objectsOnGrid)
        {
            var placeableObject = boardObject.GetComponent<PlaceableObject>();
            if (placeableObject != null)
            {
                if (!placeableObject.CheckPlaceableObjectSolution())
                    return false;
            }
        }
        return true;
    }



}
