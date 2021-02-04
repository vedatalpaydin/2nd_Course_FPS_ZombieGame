using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    private float speed = 0.1f;
    private float xSensitivity = 2f;
    private float ySensitivity = 2f;
    private float minX = -90f;
    private float maxX = 90f;

    private Quaternion characterRot;
    private Quaternion cameraRot;

    private Rigidbody rb;
    private CapsuleCollider capsule;

    void Start()
    {
        rb =GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(0,300,0);
        }
        
    }

    private void FixedUpdate()
    {
        float yRot = Input.GetAxis("Mouse X") * xSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * ySensitivity;
        
        cameraRot *= Quaternion.Euler(-xRot,0,0);
        characterRot *= Quaternion.Euler(0,yRot,0);

        cameraRot = ClampRotationAroundXAxis(cameraRot);
        
        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;

        float x = Input.GetAxis("Horizontal")*speed;
        float z = Input.GetAxis("Vertical")*speed;

        transform.position +=cam.transform.forward * z + cam.transform.right * x;//new Vector3(x, 0, z);
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, minX, maxX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }    

    bool IsGrounded()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position,capsule.radius,Vector3.down,out hitInfo,(capsule.height/2f)-capsule.radius+0.1f))
        {
            return true;
        }
        return false;
    }
}
