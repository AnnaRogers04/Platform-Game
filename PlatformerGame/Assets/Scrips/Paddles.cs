using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddles : MonoBehaviour
{
    [SerializeField] private GameObject pivotObject;
    [SerializeField] private bool Rotation;
    [SerializeField] private float min = 0f;
    [SerializeField] private float max = 90f;
    private float temp;
    private bool RoatateAround;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && RoatateAround ==false) //just to get input
        {
            RoatateAround = true;
        }


        if (RoatateAround)
        {
            if (Rotation) //should rotate left
            {
                gameObject.transform.RotateAround(pivotObject.transform.position, Vector3.left, Mathf.Lerp(min, max, temp ));   
            }
            else
            {
                gameObject.transform.RotateAround(pivotObject.transform.position, Vector3.right, Mathf.Lerp(min, max, temp));
            }
            
            temp += 0.5f * Time.deltaTime;
        } //after you complete rotation reset roatatearound to false;

        if (temp >= max)
        {
            gameObject.transform.RotateAround(pivotObject.transform.position, Vector3.left, Mathf.Lerp(max, min, temp )); 
        }
        else
        {
            gameObject.transform.RotateAround(pivotObject.transform.position, Vector3.right, Mathf.Lerp(max, min, temp));
        }    
    }
}
    
