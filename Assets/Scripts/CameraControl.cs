/*
 Thanks to Gone2Plaid comment on
 https://forum.unity.com/threads/click-drag-camera-movement.39513/
 which is a code modified version from a video
 https://www.youtube.com/watch?v=mJCbEL5J5fg
 Original code sourced from LostConjugate & Matti Jokihaara
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float scale = 5f;
    public float dragSpeed = 15f;
    public float normalizedScale = 250f;

    private void LateUpdate()
    {
        //Zoom
        scale -= Input.mouseScrollDelta.y;
        scale = Mathf.Clamp(scale, 0.25f, 50f);
        Camera.main.orthographicSize = scale;
        //Drag pan
        Vector3 position = transform.position;
        if (Input.GetMouseButton(1))
        {
            position.x -= Input.GetAxis("Mouse X") * dragSpeed * scale / normalizedScale;
            position.y -= Input.GetAxis("Mouse Y") * dragSpeed * scale / normalizedScale;
        }
        transform.position = position;
    }
}
