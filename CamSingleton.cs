using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSingleton : Singleton<CamSingleton>
{
    private Vector3 rotationVector;
    private Quaternion defaultRotation;
    void Start()
    {
        defaultRotation = transform.localRotation;
    }
    
    public void RestoreDefaultRotation()
    {
        transform.localRotation = defaultRotation;
    }

    void Update()
    {
        xRotate();
    }
    private void xRotate()
    {
        if (Input.GetMouseButton(1))
        {
            GameManager.instance.rotateUsingAxis(ref rotationVector, this);
            Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
        }
    }
}
