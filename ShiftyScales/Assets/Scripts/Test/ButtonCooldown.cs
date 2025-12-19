using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCooldown : MonoBehaviour
{

    public float cooldownDuration;

    private bool cooldown;
    private Button myButton;

    void Awake()
    {
        myButton = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked() 
    {
        if ( cooldown == false ) 
        {
            // Disable button
            if (myButton != null)
                myButton.interactable = false;
            Invoke("ResetCooldown", cooldownDuration);
            cooldown = true;
        }
    }

    void ResetCooldown()
    {
        // Enable button
            if (myButton != null)
                myButton.interactable = true;
        cooldown = false;
    }

}
