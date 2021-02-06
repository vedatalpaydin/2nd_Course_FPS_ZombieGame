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
    public AudioSource jump;
    public AudioSource land;
    public AudioSource ammoPickup;
    public AudioSource medKitPickup;
    public AudioSource triggerSound;
    public AudioSource deathSound;
    public AudioSource reloadSound;
    
    private float speed = 0.1f;
    private float xSensitivity = 2f;
    private float ySensitivity = 2f;
    private float minX = -90f;
    private float maxX = 90f;
    private float x;
    private float z;

    private int ammo = 0;
    private int maxAmmo = 50;
    private int ammoClip = 0;
    private int ammoClipMax = 10;
    private int health = 0;
    private int maxHealth = 100;
    
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

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            anim.SetBool("arm", !anim.GetBool("arm"));
        if (Input.GetMouseButtonDown(0) && !anim.GetBool("fire"))
        {
            if (ammoClip>0)
            {
                ammoClip--;
                anim.SetTrigger("fire");
            }
            else if (anim.GetBool("arm"))
                triggerSound.Play();

        }

        if (Input.GetKeyDown(KeyCode.R) && anim.GetBool("arm"))
        {
            anim.SetTrigger("reload");
            reloadSound.Play();
            int amountNeded = ammoClipMax - ammoClip;
            int ammoAvailable = amountNeded < ammo ? amountNeded : ammo;
            ammo -= ammoAvailable;
            ammoClip += ammoAvailable;
        }
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
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(0,300,0);
            jump.Play();
            if(anim.GetBool("walking"))
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag=="Ammo" && ammo < maxAmmo)
        {
            ammo = Mathf.Clamp(ammo + 10, 0, maxAmmo);
            ammoPickup.Play();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag=="MedKit" && health < maxHealth)
        {
            health = Mathf.Clamp(health + 10, 0, maxHealth);
            medKitPickup.Play();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag=="Lava")
        {
            health -= 25;
                if(health<=0)
                    deathSound.Play();
        }
        if (IsGrounded())
        {
            if(anim.GetBool("walking"))
                InvokeRepeating(nameof(PlayFootstepAudio),0,0.4f);
            land.Play();
        }
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
