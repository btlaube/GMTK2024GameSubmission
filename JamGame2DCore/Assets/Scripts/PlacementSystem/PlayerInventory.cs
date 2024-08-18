using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject heldObject;
    [SerializeField] private Transform heldObjectParent;
    [SerializeField] private List<GameObject> materialsInventory;

    private void Awake()
    {
        materialsInventory = new List<GameObject>();
    }

    public void HoldMovableObject(GameObject objectToEquip)
    {
        if (objectToEquip != null) heldObject = objectToEquip;
    }

    public void PlaceHeldObject()
    {
        heldObject = null;
    }

    public void PickUpMaterial(GameObject material)
    {
        materialsInventory.Add(material);
    }

}
