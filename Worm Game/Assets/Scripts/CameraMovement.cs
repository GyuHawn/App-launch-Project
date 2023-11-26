using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 offset;

    private void Start()
    {
        offset = new Vector3(0, 2, -3);
    }
}
