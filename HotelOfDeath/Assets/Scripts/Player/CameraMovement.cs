using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    [Header("Mouse Activity: ")] 
    [SerializeField] [Range(0, 500)] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;
    private float _xRotation;

    private float _mouseX;
    private float _mouseY;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        _mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        _mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90, 90);
        
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * _mouseX);
    }
}
