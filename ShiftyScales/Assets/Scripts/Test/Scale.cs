using UnityEngine;

public class Scale : MonoBehaviour
{
    public float scaleRate;

    public float scrollInput;
    public bool isMouseOver;



    void Update()
    {
        // Get the scroll wheel input
        scrollInput = Input.GetAxis("Mouse ScrollWheel");
        // Debug.Log(scrollInput);

        if (isMouseOver && scrollInput != 0.0f)
        {
            // Check if the Enter key (Return) is pressed and scroll input is not zero
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log($"Holding Enter and scrolling: {scrollInput}");
                // Calculate the scale change
                Vector3 scaleChange = new Vector3(0.0f, scrollInput, 0.0f) * scaleRate;

                // Apply the scale change
                transform.localScale += scaleChange;

                // OClamp the scale to avoid it becoming too small or too large
                transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0.1f, 10f), Mathf.Clamp(transform.localScale.y, 0.1f, 10f), Mathf.Clamp(transform.localScale.z, 0.1f, 10f));
            }

            else if (Input.GetKey(KeyCode.W))
            {
                Debug.Log($"Holding Control and scrolling: {scrollInput}");
                // Calculate the scale change
                Vector3 scaleChange = new Vector3(scrollInput, 0.0f, 0.0f) * scaleRate;

                // Apply the scale change
                transform.localScale += scaleChange;

                // OClamp the scale to avoid it becoming too small or too large
                transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0.1f, 10f), Mathf.Clamp(transform.localScale.y, 0.1f, 10f), Mathf.Clamp(transform.localScale.z, 0.1f, 10f));
            }
        }
    }

    void OnMouseEnter()
    {
        Debug.Log("outch");
        isMouseOver = true;
    }

    void OnMouseExit()
    {
        isMouseOver = false;
    }
}
