
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

# The Process
The process for the paddles was trial and error, below is the 1st code sample I created in an attempt to get the paddles moving.
I know I wanted the ball to 

## Music 
For this game I added upbeat 8-bit theme music like in arcades. I wanted to generate that arcade retro feel in the game to simulate a pinball machine. This adds a level of fun and annoyance to the game as the longer the game plays the more that same tune will become more and more rage educing. 

## particle
I added particles to the bottom of my game to act as a target for where you (the player) should aim to end up. I made them red so they stood out from the main game and are clear to see for the player. On top of the particles i also added a collider, when hit this collider leads to a win screen.

## Scenes
I decided I wanted a main menu screen on my game, that way it looked more professional and doesn't immediately open the game. I think this makes the game look nicer as i didn't focus on graphics.
Along with my main menu scene I added a win scene that's triggered when the player collides with the collider I added onto the particle system.
Along with this i also added a main menu button onto my pause screen. 
My win screen has a play again and main menu button that's fully functional and my main menu has a play and quit game button, both of which also work.

![alt text](<https://raw.githubusercontent.com/AnnaRogers04/Platform-Game/refs/heads/main/Particle%20ss%20for%20dev%20journal.png>)

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



- [https://lunarlynx-games.itch.io/bitball-bounce](itchlink)
- [https://github.com/AnnaRogers04](gitlink)
- [https://youtu.be/bjTTuAMwsLE?si=PC70dI8VUV12Xpg-](youtubelink)





## Reflection

### What Went Well
It took me less time to make this game then the last one. I cared less about the visuals and more about the quality of code I was producing. I made sure this game was completed a lot sooner then the last game.

### What Did Not Go Well
During the process of this game I really struggled making the paddles work, the code was frustrating and even trying to simplify it wasn't easy. I eventually made a code that worked with the help of Assad. I used bounce pads to add the force the paddles weren't adding properly.

### What would you do differently next time?
If i had the time i would have researched more into hinge joints and attempted to make it work more. Unfortunately due to time constraints I had to change my approach to the paddles.

# Documentation
I wanted to create a paddle system for my game. I was originally going to use hinge joints but after some difficulties with the system I attempted to code the paddles, this eventually worked. After adding obstacles with bounce pads and rotating objects the game came together really well.

# Implementation

## What was the process of completing the task? What influenced your decision making?
When making my game i was given a concept to base my game off of. Through this concept i decided to create a physics base game where my more difficult code was focused on the paddles. I decided to use physics because ive never used physics before and wanted to learn something new.

## What was the process of completing the task at hand? Did you do any initial planning?
I didn't have any initial concept for my game. I knew that i was making a pinball game but i didn't know how i wanted to lay it out or the obstacles or how the game itself was going to run. The more i put into the scene the more the project came together.

## Did you receive any feedback from users, peers or lecturers? How did you react to it?
The feedback i received was that the angled world i made was a big issue when making the paddles. After a while i agreed that the world was the issue and straightened it, making everything a lot more easier.

### What creative or technical approaches did you use or try, and how did this contribute to the outcome?
I attempted to use a hinge joint for my paddles, eventually the hinge joint wasn't working as expected and i had to change my idea. The hinge joint didn't contribute to my final project. I also learnt how to add music and particles to the game. The music has an upbeat tune making the game really immersive, while the particles act as a target for the player. I think this went well and added to the overall game.


### Did you have any technical difficulties? If so, what were they and did you manage to overcome them?
I had a lot of technical issues with the paddles. They weren't moving how I wanted them to, when they did move there wasn't enough upwards force to push the ball up back into the game. For this i changed the speed of the paddles and added bounce pads to add the force desired. 

## Bibliography
Technologies, U. (s.d.) Unity - Manual: Hinge Joint component reference. At: https://docs.unity3d.com/6000.0/Documentation/Manual/class-HingeJoint.html (Accessed  29/11/2024).

Unity3D Pinball Tutorial part III- The plunger (2017) At: https://www.youtube.com/watch?v=LXOy2uWjJFQ (Accessed  29/11/2024).


## Declared Assets
ChatGBT 3.5 
8 Bit Retro Game Music | Royalty-free Music (s.d.) At: https://pixabay.com/music/video-games-8-bit-retro-game-music-233964/ (Accessed  05/12/2024).
Stylized Tiles Texture | 2D Tiles | Unity Asset Store (s.d.) At: https://assetstore.unity.com/packages/2d/textures-materials/tiles/stylized-tiles-texture-192876 (Accessed  05/12/2024).






