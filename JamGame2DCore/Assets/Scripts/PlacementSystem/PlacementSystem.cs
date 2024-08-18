using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    private PlayerInventory playerInventory;
    private GridManager gridManager;

    private Camera m_Camera;

    private Vector3 mousePosition;
    private Vector3Int gridPosition;
    private bool holdingObject = false;
    private GameObject selectedObject = null;

    void Awake()
    {
        m_Camera = Camera.main;
        playerInventory = GetComponentInParent<PlayerInventory>();
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
    }

    private void Update()
    {
        mousePosition = inputManager.GetSelectedMapPositionMouse();
        gridPosition = grid.WorldToCell(mousePosition);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log((gridPosition));
            TryPickUpObject();
        }
        else if (Input.GetMouseButton(0) && holdingObject)
        {
            DragObject();
        }
        else if (Input.GetMouseButtonUp(0) && holdingObject)
        {
            PlaceObject();
        }
    }

    private void TryPickUpObject()
    {
        if (gridManager.objectsOnGrid.Count > 0)
        {
            foreach (GameObject boardObject in gridManager.objectsOnGrid)
            {
                // Check if object is PlaceableObject and that it contains the selected grid cell
                if (boardObject.GetComponentInParent<PlaceableObject>() != null && boardObject.GetComponentInParent<PlaceableObject>().ContainsCell(gridPosition))
                {
                    // Determine selected object
                    selectedObject = boardObject;
                    playerInventory.HoldMovableObject(selectedObject);
                    PlaceableObject placeableObject = selectedObject.GetComponentInParent<PlaceableObject>();
                    if (placeableObject != null)
                    {
                        placeableObject.StartPlacingObject();
                        holdingObject = true;
                    }
                    return;
                }                
            }
        }
    }

    private void DragObject()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.position = grid.CellToWorld(gridPosition);
        }
    }

    private void PlaceObject()
    {
        if (CheckPlacement(selectedObject))
        {
            selectedObject.GetComponent<PlaceableObject>().StopPlacingObject();
            holdingObject = false;
            selectedObject = null;
        }
        // If invalid position reset to where it was picked up
    }

    private bool CheckPlacement(GameObject objectToPlace)
    {
        PlaceableObject placeableObject = objectToPlace.GetComponent<PlaceableObject>();
        Vector3Int newGridPosition = grid.WorldToCell(mousePosition);

        // Check if the placement is valid
        if (placeableObject.IsPositionValid(newGridPosition))
        {
            return true;
        }
        return false;
    }
}
