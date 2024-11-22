using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float forceStrength = 100f; // Force applied to move the paddle
    public float returnForceStrength = 50f; // Force to correct out-of-bounds rotation
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
        // Apply force to move paddle towards max or min rotation based on input
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyForceTowards(maxRotation);
        }
        else
        {
            ApplyForceTowards(minRotation);
        }

        // Check and correct if the rotation goes out of bounds
        CheckAndCorrectOutOfBounds();
    }

    void ApplyForceTowards(float targetRotation)
    {
        // Get the current rotation of the paddle in local space
        float currentRotation = transform.localEulerAngles.x;
        if (currentRotation > 180f) currentRotation -= 360f; // Convert to a range of -180 to 180

        // Determine the direction to apply force
        float direction = targetRotation > currentRotation ? 1f : -1f;

        // Calculate torque for rotation
        Vector3 torque = transform.right * direction * forceStrength * 10000f;

        // Apply torque to the Rigidbody
        _rigidbody.AddTorque(torque);

        // Stop applying force if close to the target rotation
        if (Mathf.Abs(targetRotation - currentRotation) < 1f)
        {
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    void CheckAndCorrectOutOfBounds()
    {
        // Get the current rotation of the paddle in local space
        float currentRotation = transform.localEulerAngles.x;
        if (currentRotation > 180f) currentRotation -= 360f; // Convert to a range of -180 to 180

        // Apply correction force if out of bounds
        if (currentRotation < minRotation)
        {
            Vector3 correctionTorque = transform.right * returnForceStrength;
            _rigidbody.AddTorque(correctionTorque);
        }
        else if (currentRotation > maxRotation)
        {
            Vector3 correctionTorque = -transform.right * returnForceStrength;
            _rigidbody.AddTorque(correctionTorque);
        }
    }
}
