﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    private float speed = 0.1f;

    private Rigidbody rb;
    private CapsuleCollider capsule;

    void Start()
    {
        rb =GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
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
