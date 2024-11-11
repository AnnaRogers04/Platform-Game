using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 25f;
    private void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Debug.Log("PLEASE WORK!");
            rb.AddForce(Vector3.up * bounceForce);
        }

    }
}
