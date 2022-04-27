using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchViewer : MonoBehaviour
{

    public float xAxisThreshold = 100f;
    public float yAxisMaxDistance = 50f;
    public float zoomInDistanceTreshold = 70f;
    public float zoomInDistanceRatioTreshold = 0.2f;
    public ArtificialTouch artificialTouch = null;
    public GameObject touchViewPrefab;
    private float xDistance;
    private float yDistance;
    private Vector2 initialPosition = Vector2.zero;
    private Camera cam = null;

    private Vector2 secondTouch;
    private float distanceBetweenTouches = 0f;
    private float distanceBetweenTouchesInitial = 0f;
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        // task # 1
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                initialPosition = touch.position;
                touchViewPrefab.SetActive(true);
            }
            if(touchViewPrefab.activeSelf)
            {
                touchViewPrefab.transform.position = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                // Debug.DrawRay(
                //     artificialTouch.transform.position,
                //     touchViewPrefab.transform.position - artificialTouch.transform.position
                //     );
            }
            xDistance = touch.position.x - initialPosition.x;
            yDistance = touch.position.y - initialPosition.y;
            if (xDistance > xAxisThreshold && Mathf.Abs(yDistance) < yAxisMaxDistance)
            {
                Debug.Log("RIGHT SWIPE");
            }
            else if (xDistance < (xAxisThreshold * -1) && Mathf.Abs(yDistance) < yAxisMaxDistance)
            {
                Debug.Log("LEFT SWIPE");
            }
            if(touch.phase == TouchPhase.Ended)
            {
                touchViewPrefab.SetActive(false);
            }
            // task # 2
            if ( artificialTouch != null )
            {
                secondTouch = artificialTouch.touchPos;
                distanceBetweenTouches = Vector2.Distance(secondTouch, touch.position);
                if(touch.phase == TouchPhase.Began)
                {
                    distanceBetweenTouchesInitial = distanceBetweenTouches;
                }
                if(touch.phase == TouchPhase.Ended)
                {
                    distanceBetweenTouchesInitial = 0f;
                }
                if( 
                        ((distanceBetweenTouches - distanceBetweenTouchesInitial) > zoomInDistanceTreshold)
                        || 
                        ((distanceBetweenTouchesInitial * (1 + zoomInDistanceRatioTreshold)) < distanceBetweenTouches)
                    )
                {
                    Debug.Log("ZOOM IN DETECTED");
                }
            }
        }
    }
}
