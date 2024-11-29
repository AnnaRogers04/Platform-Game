
# Platformer Game
Fundamentals Of Games Development

Anna Rogers

2315276

## Pinball Research
For my pinball game I decided to create working paddles that shoot the ball up. For this i needed to look into different ways to make these paddles work. For this i looked into hinge joints and ApplyForceTowards(float targetRotation) to attempt an easier way for the same system.

### What sources or references have you identified as relevant to this task?
i used this a source on unity documentation to look into hinge joints further. I found that its component that groups 2 rigidbodies together, constraining them to move like they are connected by a hinge.(Technologies, s.d.) 
Ive looked deep into hinge joints and even tried to put one on my paddles, however it was a lot harder then I first thought and ended up not working how i wanted it to.
I also followed a youtube video about hinge joints that i found helpful, but the paddles still didn't work for me.
<iframe width="560" height="315" src="https://www.youtube.com/embed/LXOy2uWjJFQ?si=sCcp51az950O3HGa" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>

https://www.youtube.com/watch?v=LXOy2uWjJFQ 

(Unity3D Pinball Tutorial part III- The plunger, 2017)

### Concept
For my concept I wanted to create a pinball-like game that has functioning paddles and obstacles. I wanted the obstacles to have bounce pads so the ball darts around the scene I created like a real pinball machine.

## The Process
The process for the paddles was trial and error, below is the 1st code sample I created in an attempt to get the paddles moving.


### Paddles.cs

```cs
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


```
This code didn't the paddles weren't moving and because the person who helped me worked selected days I was unable to ask for help to fix the code.

## Final Outcomes
### BouncePad.cs
```cs
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
```
### Paddles.cs
```cs
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

```
### RotatingObject.cs
```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public Vector3 rotationDirection = Vector3.up;
    public float rotationSpeed = 50f;

    private void Update()
    {
        transform.Rotate(rotationDirection.normalized, rotationSpeed * Time.deltaTime);
    }
}
```

This code didn't the paddles weren't moving and because the person who helped me worked selected days I was unable to ask for help to fix the code.





- [LINK TO GAME](itchlink)
- [](gitlink)
- [LINK TO DEMONSTRATION VIDEO](youtubelink)



## Reflection

### What Went Well
It took me less time to make this game then the last one. I cared less about the visuals and more about the quality of code I was producing. I made sure this game was completed a lot sooner then the last game.

### What Did Not Go Well
During the process of this game I really struggled making the paddles work, the code was frustrating and even trying to simplify it wasn't easy. I eventually made a code that worked with the help of Assad. I used bounce pads to add the force the paddles weren't adding properly.

### What would you do differently next time?
If i had the time i would have researched more into hinge joints and attempted to make it work more. Unfortunately due to time constraints I had to change my approach to the paddles.

## Bibliography
Technologies, U. (s.d.) Unity - Manual: Hinge Joint component reference. At: https://docs.unity3d.com/6000.0/Documentation/Manual/class-HingeJoint.html (Accessed  29/11/2024).

Unity3D Pinball Tutorial part III- The plunger (2017) At: https://www.youtube.com/watch?v=LXOy2uWjJFQ (Accessed  29/11/2024).


## Declared Assets
ChatGBT 3.5 


# Documentation





# Implementation

## What was the process of completing the task? What influenced your decision making?

## What was the process of completing the task at hand? Did you do any initial planning?

## Did you receive any feedback from users, peers or lecturers? How did you react to it?


### What creative or technical approaches did you use or try, and how did this contribute to the outcome?



### Did you have any technical difficulties? If so, what were they and did you manage to overcome them?






