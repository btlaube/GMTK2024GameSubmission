using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnplaceableGridObject : GridObject
{
    // Additional properties or methods specific to unplaceable objects can be added here

    protected override void Awake()
    {
        base.Awake();
        // Initialization specific to unplaceable objects, if needed
    }

    protected override void Update()
    {
        base.Update();
        // Logic specific to unplaceable objects, if needed
    }
}
