//This script handles situations when certain objects flies out of screen
using UnityEngine;

public class BoundsBehaviour : MonoBehaviour
{
    //Set in inspector
    public float    radius = 1f; //Approximate size of object in Unity units

    //Set dynamically
    public float    _camHalfWidth;
    public float    _camHalfHeight;

    void Awake()
    {
        _camHalfHeight = Camera.main.orthographicSize;
        _camHalfWidth = _camHalfHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        if (pos.x > _camHalfWidth + radius / 2)
        {
            pos.x = -(_camHalfWidth + radius / 2);
        }
        if (pos.x < -(_camHalfWidth + radius / 2))
        {
            pos.x = _camHalfWidth + radius / 2;
        }
        if (pos.y > _camHalfHeight + radius / 2)
        {
            pos.y = -(_camHalfHeight + radius / 2);
        }
        if (pos.y < -(_camHalfHeight + radius / 2))
        {
            pos.y = _camHalfHeight + radius / 2;
        }
        transform.position = pos;
    }
}
