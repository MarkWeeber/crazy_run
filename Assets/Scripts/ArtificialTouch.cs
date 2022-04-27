using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialTouch : MonoBehaviour
{
    private TextMesh artificialTouchText;
    private Transform touchTransform = null;
    private Camera cam = null;
    private Vector3 screenPos = Vector3.zero;
    public Vector2 touchPos
    {
        get {return new Vector2(screenPos.x, screenPos.y);}
    }
    void Start()
    {
        artificialTouchText = GetComponent<TextMesh>();
        touchTransform = this.transform.parent.GetComponent<Transform>();
        cam = Camera.main;
        UpdateTouchPosition();
    }

    void Update()
    {
        UpdateTouchPosition();
    }

    private void UpdateTouchPosition()
    {
        artificialTouchText.text = "0, 1";
        screenPos = cam.WorldToScreenPoint(touchTransform.position);
        artificialTouchText.text = screenPos.x.ToString("#.##") + ", " + screenPos.y.ToString("#.##");
    }
}
