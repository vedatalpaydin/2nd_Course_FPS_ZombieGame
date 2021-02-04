using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FPController : MonoBehaviour
{
    public Camera cam;
    public Animator anim;

    public AudioSource[] footsteps; 
    
    private float speed = 0.1f;
    private float xSensitivity = 2f;
    private float ySensitivity = 2f;
    private float minX = -90f;
    private float maxX = 90f;
    private float x;
    private float z;

    private Quaternion characterRot;
    private Quaternion cameraRot;

    private Rigidbody rb;
    private CapsuleCollider capsule;

    private bool lockCursor = true;
    private bool cursorIsLocked = true;

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
        if (Input.GetKeyDown(KeyCode.F))
            anim.SetBool("arm", !anim.GetBool("arm"));
        if (Input.GetMouseButtonDown(0))
            anim.SetTrigger("fire");
        if (Input.GetKeyDown(KeyCode.R))
            anim.SetTrigger("reload");
        if (Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0)
        {
            if (!anim.GetBool("walking"))
            {
                anim.SetBool("walking", true); 
                InvokeRepeating(nameof(PlayFootstepAudio),0,0.4f);
            }
        }
        else if (anim.GetBool("walking"))
        {
            anim.SetBool("walking",false);
            CancelInvoke(nameof(PlayFootstepAudio));
        }
    }

    void PlayFootstepAudio()
    {
        AudioSource audioSource = new AudioSource();
        var n = Random.Range(0, footsteps.Length);
        audioSource = footsteps[n];
        audioSource.Play();
        footsteps[n] = footsteps[0];
        footsteps[0] = audioSource;
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

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(0,300,0);
        }
        
        x = Input.GetAxis("Horizontal")*speed;
        z = Input.GetAxis("Vertical")*speed;

        transform.position +=cam.transform.forward * z + cam.transform.right * x;//new Vector3(x, 0, z);
        UpdateCursorLock();
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

    public void SetCursorLocked(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        if (lockCursor)
        {
            InternalLockUpdate();
        }
    }

    public void InternalLockUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cursorIsLocked = true;
        }

        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
