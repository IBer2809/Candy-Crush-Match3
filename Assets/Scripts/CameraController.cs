using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Start()
    {
        Vector2 camPos = GridManager.Instance.GetGridSize();
        transform.position = new Vector3(camPos.x / 2 - 0.5f, camPos.y / 2 - 0.5f, transform.position.z);
        Camera.main.orthographicSize = camPos.x + 1;
    }
}
