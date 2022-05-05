using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTouchMover : MonoBehaviour
{
    // publics
    public bool active = false;
    // settings
    [SerializeField]
    private float touchTreshold = 20f;
    [SerializeField]
    private float speedMultiplier = 1f;
    [SerializeField]
    private float maxVelocity = 20f;
    // privates
    private Vector2 touchDirection = Vector2.zero;
    private Vector2 initialTouchPosition = Vector2.zero;
    private Rigidbody rb = null;
    private Vector3 resultingVelocity = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(active)
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    initialTouchPosition = touch.position;
                }
                touchDirection = touch.position - initialTouchPosition;
            }
            else
            {
                touchDirection = Vector2.zero;
            }
        }
        else
        {
            touchDirection = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if(touchDirection.magnitude > touchTreshold && rb != null)
        {
            resultingVelocity = new Vector3(touchDirection.y * -1, 0, touchDirection.x).normalized * speedMultiplier;
            rb.AddForce(resultingVelocity);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }
}
