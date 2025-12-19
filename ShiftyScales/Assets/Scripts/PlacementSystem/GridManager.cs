using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<GameObject> objectsOnGrid;

    private void Start()
    {
        objectsOnGrid = new List<GameObject>();
        SetList();
    }

    private void Update()
    {
        UpdateList();
    }

    public void UpdateList()
    {
        ClearList();
        SetList();
    }

    private void SetList()
    {
        foreach (Transform child in transform)
        {
            objectsOnGrid.Add(child.gameObject);
        }
    }

    private void ClearList()
    {
        objectsOnGrid.Clear();
    }

}
