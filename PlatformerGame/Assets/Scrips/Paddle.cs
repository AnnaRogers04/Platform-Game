using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float forceStrength = 100f; // Force applied to the paddle
    public float returnForceStrength = 50f; // Force for returning to min rotation
    public float minRotation = -30f; // Minimum allowable rotation on the X-axis
    public float maxRotation = 30f;  // Maximum allowable rotation on the X-axis
    private Rigidbody _rigidbody;

    void Start()
    {
        // Get the Rigidbody component
        _rigidbody = GetComponent<Rigidbody>();

        // Freeze unnecessary axes to constrain paddle movement
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ |
                                 RigidbodyConstraints.FreezeRotationZ |
                                 RigidbodyConstraints.FreezeRotationY |
                                 RigidbodyConstraints.FreezePositionX |
                                 RigidbodyConstraints.FreezePositionY;
    }

    void Update()
    {
        // Check if the space bar is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyForceTowards(maxRotation);
        }
        else
        {
            ApplyForceTowards(minRotation);
        }
    }

    void ApplyForceTowards(float targetRotation)
    {
        // Get the current rotation of the paddle in local space
        float currentRotation = transform.localEulerAngles.x;
        if (currentRotation > 180f) currentRotation -= 360f; // Convert to a range of -180 to 180

        // Determine the direction to apply force
        float direction = targetRotation > currentRotation ? 1f : -1f;

        // Calculate the force direction along the X-axis
        Vector3 torque = transform.right * direction * (forceStrength * 1000000000f);

        // Apply torque to the rigidbody
        _rigidbody.AddTorque(torque * 10000f);

        // Stop applying force if close to the target rotation
        if (Mathf.Abs(targetRotation - currentRotation) < 1f)
        {
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
