using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavCamMove : MonoBehaviour
{
    public GameObject ground;
    private float v, h, d;
    private const int speed = 2;
    private Rigidbody rb;
    Vector3 rotationVector ;
    void Start()
    {
        transform.position = new Vector3(ground.transform.position.x + 126, ground.transform.position.y + 2, ground.transform.position.z - 190);
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        moveWithTranslate();
        //moveWithRigid();
    }

    //private RaycastHit rh;
    //private void instantiateByChoosing()
    //{
    //    if (!isInstanciated)
    //    {
    //        if (Input.GetMouseButtonDown(0) &&
    //            (!GameManager.instance.ctr.activeSelf && !GameManager.instance.settings.activeSelf))
    //        {
    //            Ray r = myCam_x.ScreenPointToRay(Input.mousePosition);
    //            if (Physics.Raycast(r, out rh, Mathf.Infinity))
    //            {
    //                connectAndStart();
    //            }
    //        }
    //    }
    //    if (Input.GetMouseButtonUp(0))
    //    {

    //    }
    //}


    void takeTranslateInputs()
    {
        h = Input.GetAxis("Horizontal") * speed;
        v = Input.GetAxis("Vertical") * speed;
        d = Input.GetAxis("Depth") * speed;
    }

    void moveWithTranslate()
    {
        takeTranslateInputs();
        
        h = (transform.position.x < 248 && transform.position.x > -248) ? h : (transform.position.x > 248) ? -5 : 5;
        v = (transform.position.y < 0.5) ? 0.5f : v;
        d = (transform.position.z < 220 && transform.position.z > -250) ? d : (transform.position.z > 220) ? -5 : 5;

        transform.Translate(new Vector3(h, v, d));
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

    void moveWithRigid()
    {
        takeTranslateInputs();
        Vector3 targetPos = (transform.forward * v + transform.right * h + transform.up * d);
        rb.MovePosition(transform.position + targetPos);
        xRotate();
    }
    
}
