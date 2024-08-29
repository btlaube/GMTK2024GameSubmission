using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTransfer : MonoBehaviour
{
    [SerializeField] protected Grid grid;

    public Transform objectOne;
    public Transform objectTwo;
    [SerializeField] private Collider2D leftGridCollider;
    [SerializeField] private Collider2D rightGridCollider;

    public float scaleRate;

    // Track the grid cells for each "chamber"
    [SerializeField] private Vector3Int[] leftOccupiedCells;
    [SerializeField] private Vector3Int[] rightOccupiedCells;
    // [SerializeField] private Collider2D leftGridBlocker;
    // [SerializeField] private Collider2D rightGridBlocker;

    void Start()
    {
        UpdateOccupiedCells();
        ActivateChamber(leftGridCollider);
        ActivateChamber(rightGridCollider);
    }

    void Update()
    {
        // UpdateObjectsInChambers();
    }

    public void UpdateOccupiedCells()
    {
        List<Vector3Int> leftOccupiedPositions = new List<Vector3Int>();
        List<Vector3Int> rightOccupiedPositions = new List<Vector3Int>();

        leftOccupiedPositions = GetOccupiedPositionsForBoxCollider(leftGridCollider as BoxCollider2D);
        rightOccupiedPositions = GetOccupiedPositionsForBoxCollider(rightGridCollider as BoxCollider2D);

        leftOccupiedCells = leftOccupiedPositions.ToArray();
        rightOccupiedCells = rightOccupiedPositions.ToArray();
    }

    private List<Vector3Int> GetOccupiedPositionsForBoxCollider(BoxCollider2D boxCollider)
    {
        List<Vector3Int> occupiedPositions = new List<Vector3Int>();

        Bounds bounds = boxCollider.bounds;
        // Collider offset
        bounds.extents -= new Vector3(0.2f, 0.2f, 0.0f);

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

    public void UpdateObjectsInChambers()
    {
        // Reset objectOne and objectTwo
        objectOne = null;
        objectTwo = null;

        // Check objects in the left chamber
        foreach (GameObject boardObject in FindObjectOfType<GridManager>().objectsOnGrid)
        {
            if (leftGridCollider.OverlapPoint(boardObject.transform.position))
            {
                // Debug.Log($"fart {boardObject.transform.position}");
                if (boardObject.transform == leftGridCollider.transform)
                {
                    objectOne = null;
                }
                else
                {
                    objectOne = boardObject.transform;
                }
                break; // Assume only one object can be in the chamber
            }
        }

        // Check objects in the right chamber
        foreach (GameObject boardObject in FindObjectOfType<GridManager>().objectsOnGrid)
        {
            if (rightGridCollider.OverlapPoint(boardObject.transform.position))
            {
                if (boardObject.transform == rightGridCollider.transform)
                {
                    objectTwo = null;
                }
                else
                {
                    objectTwo = boardObject.transform;
                }
                break; // Assume only one object can be in the chamber
            }
        }

        // If either chamber has one object, deactivate the chamber
        if (objectOne != null)
        {
            DeactivateChamber(leftGridCollider);
        }
        else
        {
            ActivateChamber(leftGridCollider);
        }

        if (objectTwo != null)
        {
            DeactivateChamber(rightGridCollider);
        }
        else
        {
            ActivateChamber(rightGridCollider);
        }
    }

    private void DeactivateChamber(Collider2D chamberCollider)
    {
        // Enable a GameObject in the grid by setting it active
        Transform chamberObject = chamberCollider.transform;
        // chamberObject.GetComponent<UnplaceableGridObject>().enabled = true;
        // chamberObject.SetActive(true);

        chamberObject.parent = grid.transform;;

        // Debug.Log($"Deactivated {chamberCollider}");

        // Update the grid manager to reflect the changes
        GridManager gridManager = FindObjectOfType<GridManager>();
        gridManager.UpdateList();
    }

    private void ActivateChamber(Collider2D chamberCollider)
    {
        // Enable a Transform in the grid by changing the parent
        Transform chamberObject = chamberCollider.transform;
        chamberObject.parent = transform;

        // Update the grid manager to reflect the changes
        GridManager gridManager = FindObjectOfType<GridManager>();
        gridManager.UpdateList();
    }

    public void SwapScales()
    {
        if (objectOne == null || objectTwo == null) return;
        // Compute scale factors
        Vector3 targetScaleOne = objectTwo.localScale;
        Vector3 targetScaleTwo = objectOne.localScale;

        // Start the coroutine to gradually swap the scales
        StartCoroutine(SwapScalesCoroutine(targetScaleOne, targetScaleTwo));
    }

    public void MultiplyScales()
    {
        // Compute the multiplied scales
        Vector3 targetScaleOne = Vector3.Scale(objectOne.localScale, objectTwo.localScale);
        Vector3 targetScaleTwo = Vector3.Scale(objectTwo.localScale, objectOne.localScale);
        targetScaleOne = RoundVector3(targetScaleOne, 1);
        targetScaleTwo = RoundVector3(targetScaleTwo, 1);

        // Start the coroutine to gradually apply the multiplied scales
        StartCoroutine(MultiplyScalesCoroutine(targetScaleOne, targetScaleTwo));
    }

	private Vector3 RoundVector3(Vector3 vector3, int decimalPlaces = 2)
	{
		float multiplier = 1;
		for (int i = 0; i < decimalPlaces; i++)
		{
			multiplier *= 10f;
		}
		return new Vector3(
			Mathf.Round(vector3.x * multiplier) / multiplier,
			Mathf.Round(vector3.y * multiplier) / multiplier,
			Mathf.Round(vector3.z * multiplier) / multiplier);
	}

    public void MatchSmallerScale()
    {
        // Match scales to the smaller of the two objects' x-axis scale
        MatchScales(objectOne.localScale.x < objectTwo.localScale.x ? 1 : 2);
    }

    public void MatchLargerScale()
    {
        // Match scales to the larger of the two objects' x-axis scale
        MatchScales(objectOne.localScale.x < objectTwo.localScale.x ? 2 : 1);
    }

    public void MatchScales(int matchObject)
    {
        // Start the coroutine to gradually swap the scales
        if (matchObject == 1)
        {
            StartCoroutine(SwapScalesCoroutine(objectOne.localScale, objectOne.localScale));
        }
        else if (matchObject == 2)
        {
            StartCoroutine(SwapScalesCoroutine(objectTwo.localScale, objectTwo.localScale));
        }
    }

    private IEnumerator MultiplyScalesCoroutine(Vector3 targetScaleOne, Vector3 targetScaleTwo)
    {
        bool objectOneDone = false, objectTwoDone = false;

        // Get the initial scales of the objects
        Vector3 initialScaleOne = objectOne != null ? objectOne.localScale : Vector3.one;
        Vector3 initialScaleTwo = objectTwo != null ? objectTwo.localScale : Vector3.one;

        while (!objectOneDone || !objectTwoDone)
        {
            if (!objectOneDone && objectOne != null)
            {
                objectOne.localScale = Vector3.MoveTowards(objectOne.localScale, targetScaleOne, scaleRate * Time.deltaTime);
                if (objectOne.localScale == targetScaleOne)
                {
                    objectOneDone = true;
                }
            }

            if (!objectTwoDone && objectTwo != null)
            {
                objectTwo.localScale = Vector3.MoveTowards(objectTwo.localScale, targetScaleTwo, scaleRate * Time.deltaTime);
                if (objectTwo.localScale == targetScaleTwo)
                {
                    objectTwoDone = true;
                }
            }

            yield return new WaitForSeconds(0.001f); // Wait until the next frame
        }
        UpdateObjectCells();
    }

    private IEnumerator SwapScalesCoroutine(Vector3 targetScaleOne, Vector3 targetScaleTwo)
    {
        bool objectOneDone = false, objectTwoDone = false;

        // Get the initial scales of the objects
        Vector3 initialScaleOne = objectOne != null ? objectOne.transform.localScale : Vector3.one;
        Vector3 initialScaleTwo = objectTwo != null ? objectTwo.transform.localScale : Vector3.one;

        while (!objectOneDone || !objectTwoDone)
        {
            if (!objectOneDone && objectOne != null)
            {
                objectOne.localScale = Vector3.MoveTowards(objectOne.localScale, targetScaleOne, scaleRate * Time.deltaTime);
                if (objectOne.localScale == targetScaleOne)
                {
                    objectOneDone = true;
                }
            }

            if (!objectTwoDone && objectTwo != null)
            {
                objectTwo.localScale = Vector3.MoveTowards(objectTwo.localScale, targetScaleTwo, scaleRate * Time.deltaTime);
                if (objectTwo.localScale == targetScaleTwo)
                {
                    objectTwoDone = true;
                }
            }

            yield return new WaitForSeconds(0.001f); // Wait until the next frame
        }
        UpdateObjectCells();
    }

    private void UpdateObjectCells()
    {
        GridObject gridObjectOne = objectOne.GetComponent<GridObject>();
        GridObject gridObjectTwo = objectTwo.GetComponent<GridObject>();
        if (gridObjectOne != null) gridObjectOne.UpdateOccupiedCells();
        if (gridObjectTwo != null) gridObjectTwo.UpdateOccupiedCells();
    }

    public bool IsOverSwapChamber(Vector3Int gridCell)
    {
        // Check if the gridCell is within the left chamber
        foreach (Vector3Int cell in leftOccupiedCells)
        {
            // Debug.Log($"Farted: {gridCell}");
            if (cell == gridCell)
            {
                return true;
            }
        }

        // Check if the gridCell is within the right chamber
        foreach (Vector3Int cell in rightOccupiedCells)
        {
            if (cell == gridCell)
            {
                return true;
            }
        }

        return false;
    }
}
