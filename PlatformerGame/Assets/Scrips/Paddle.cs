using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float restPos = 0f, pressedPos = 45f, hitStrength = 1000f, flipperDamper = 150f;
    [SerializeField]private HingeJoint _hinge;
    // Start is called before the first frame update
    private void Start()
    {
        _hinge = GetComponent<HingeJoint>();
        _hinge.useSpring = true;
    }

    // Update is called once per frame
    private void Update()
    {
        JointSpring spring = new JointSpring();
        spring.spring = hitStrength;
        spring.damper = flipperDamper;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pressed Space");
            spring.targetPosition = pressedPos;
        }
        else
        {
            spring.targetPosition = restPos;
        }
        _hinge.spring = spring;
        _hinge.useLimits = true;
    }
}
