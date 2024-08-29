using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : GridObject
{
    [SerializeField] private InputManager inputManager;

    private bool isPlacingObject = false;
    private bool stoppedPlacing = false;
    private Vector3 initialPosition;
    private Camera m_Camera;

    // Correct solution
    [SerializeField] private Vector3 correctPos;
    [SerializeField] private Vector3 correctScale;

    private AudioHandler audioHandler;

    protected override void Awake()
    {
        base.Awake();
        m_Camera = Camera.main;

        audioHandler = GetComponent<AudioHandler>();
    }

    protected override void Update()
    {
        base.Update();

        if (isPlacingObject)
        {
            UpdateObjectPosition();
            UpdateOccupiedCells();
        }
        if (stoppedPlacing)
        {
            stoppedPlacing = false;
            UpdateOccupiedCells();
        }
    }

    public void StartPlacingObject()
    {
        if (audioHandler != null)
        {
            audioHandler.Play("PickUp");
        }

        isPlacingObject = true;
        initialPosition = transform.position;

        // Call to hold the object in the player's inventory
        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
        playerInventory.HoldMovableObject(gameObject);

        UpdateOccupiedCells();
    }

    public void StopPlacingObject()
    {
        if (audioHandler != null)
        {
            audioHandler.Play("PutDown");
        }
        isPlacingObject = false;
        stoppedPlacing = true;
    }

    private void UpdateObjectPosition()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPositionMouse();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        transform.position = grid.CellToWorld(gridPosition);
    }

    public void ReturnToInitialPosition()
    {
        // Debug.Log("Cant go there");
        if (audioHandler != null)
        {
            audioHandler.Play("Invalid");
        }
        transform.position = initialPosition;

        UpdateOccupiedCells();
    }

    public bool CheckPlaceableObjectSolution()
    {
        bool fart = (transform.position == correctPos) && (transform.localScale == correctScale);
        Debug.Log($"{transform.position} and {transform.localScale} is {fart}");
        return fart;
    }

}
