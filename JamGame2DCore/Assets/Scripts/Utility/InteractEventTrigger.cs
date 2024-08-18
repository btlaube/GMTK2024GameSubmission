using UnityEngine;
using UnityEngine.Events;

public class InteractEventTrigger : MonoBehaviour
{
    public UnityEvent myEvent;
    public Transform player;

    [SerializeField] private float interactDistance;    
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;
        
        //Check if player is within range
        if (Vector2.Distance(transform.position, player.position) < interactDistance)
        {
            // Become interactable
            if (sr != null)
                sr.enabled = true;
            // Check input from hardcoded key "X" 
            if (Input.GetKeyUp(KeyCode.X))
            {
                Interact();
                sr.enabled = false;
            }
        }
        else
        {
            // Become uninteractable
            if (sr != null)
                sr.enabled = false;
        }
    }

    private void Interact()
    {
        myEvent.Invoke();
    }
}