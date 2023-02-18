using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_UI_Animation : MonoBehaviour
{
    Camera camera_main;

    Camera camera_ui;


    private void Awake()
    {
        camera_main = Camera.main;

        camera_ui = GetComponent<Camera>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        camera_ui.transform.position = camera_main.transform.position;

        camera_ui.orthographicSize = camera_main.orthographicSize;
    }
}
