using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTransfer : MonoBehaviour
{
    public Transform objectOne;
    public Transform objectTwo;

    // [Range(-10.0f, 10.0f)]
    // public float sliderFloat;
    public float scaleRate;

    // Update is called once per frame
    void Update()
    {
        // // Compute scale factors
        // float scaleFactorOne = 1.0f - sliderFloat / 10.0f;
        // float scaleFactorTwo = 1.0f + sliderFloat / 10.0f;

        // // Apply scale factors to the game objects
        // if (objectOne != null)
        // {
        //     objectOne.transform.localScale = new Vector3(scaleFactorOne, scaleFactorOne, scaleFactorOne);
        // }

        // if (objectTwo != null)
        // {
        //     objectTwo.transform.localScale = new Vector3(scaleFactorTwo, scaleFactorTwo, scaleFactorTwo);
        // }
    }

    public void SwapScales()
    {
        // Compute scale factors
        Vector3 targetScaleOne = objectTwo.localScale;
        Vector3 targetScaleTwo = objectOne.localScale;

        // Start the coroutine to gradually swap the scales
        StartCoroutine(SwapScalesCoroutine(targetScaleOne, targetScaleTwo));
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

}
