using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    private float speed = 0.1f;
    private float xSensitivity = 5f;
    private float ySensitivity = 5f;

    private Quaternion characterRot;
    private Quaternion camRot;

    private Rigidbody rb;
    private CapsuleCollider capsule;

    void Start()
    {
        rb =GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        camRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float yRot = Input.GetAxis("Mouse X") * ySensitivity;
        float xRot = Input.GetAxis("Mouse Y") * xSensitivity;
        
        camRot *= Quaternion.Euler(-xRot,0,0);
        characterRot *= Quaternion.Euler(0,yRot,0);

        transform.localRotation = characterRot;
        cam.transform.localRotation = camRot;
        
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(0,300,0);
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        transform.position += new Vector3(x*speed, 0, z*speed);
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
