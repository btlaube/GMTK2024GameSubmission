using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    private Vector3 lastPosition;
    [SerializeField] private LayerMask placementLayermask;

    [SerializeField] private GameObject player;
    [SerializeField] private float distanceInFront = 5f;

    public Vector3 GetSelectedMapPositionMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0; // Set Z to 0 to ignore depth

        // Convert screen position to world position
        Vector2 worldPosition = sceneCamera.ScreenToWorldPoint(mousePos);

        // Debug.Log(worldPosition);
        return worldPosition;
    }

    public Vector3 GetSelectedMapPositionPlayerView()
    {
        Vector3 targetPosition = player.transform.position + player.transform.forward * distanceInFront;
        targetPosition = new Vector3(targetPosition.x, 0.0f, targetPosition.z);
        // Debug.Log(targetPosition);
        return targetPosition;
    }

}
