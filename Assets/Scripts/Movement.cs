using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    AudioSource RocketSound;
    [SerializeField] float zAngle = 0.0f;
    [SerializeField] float ThrustAmount = 0.5f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem SideThrusterLeft;
    [SerializeField] ParticleSystem SideThrusterRight;
    [SerializeField] ParticleSystem particleEngine;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        RocketSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // If spacebar is pressed, apply an upward force on the object (relative)
        Thrust();
        Rotation();
        // If the left or right arrow keys are pressed, rotate left or right respectively.
    }

    
    // Apply force in the relative up direction on spacebar
    void Thrust()
    {
        if(Input.GetKey(KeyCode.Space)) {
            StartThrust();
        }
        else {
            StopThrust();
        }
    }

    // Rotate left or right on press A or D key.
    void Rotation()
    {
        if(Input.GetKey(KeyCode.A)) {
            RotationLeft();
        }
        else if(Input.GetKey(KeyCode.D)) {
            RotationRight();
        }
        else {
            StopRotation();
        }
    }

    // Applies a rotation
    void ApplyRotation(float rotationAmount)
    {
        // Freezing rotation fixes fighting against unity's physics system.
        // Freeze our rotation, perform our rotation, then unfreeze our rotation.
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationAmount * Time.deltaTime);
        rb.freezeRotation = false;
    }

    void StartThrust()
    {
        if(!RocketSound.isPlaying) {
                RocketSound.PlayOneShot(mainEngine);
        }
        if(!particleEngine.isPlaying) {
                particleEngine.Play();
        }
        rb.AddRelativeForce(Vector3.up * ThrustAmount * Time.deltaTime);
    }

    void StopThrust()
    {
        RocketSound.Stop();
        particleEngine.Stop();
    }
    void RotationLeft()
    {
        ApplyRotation(zAngle);
        if(!SideThrusterLeft.isPlaying) {
            SideThrusterLeft.Play();
        }
    }

    void RotationRight()
    {
        ApplyRotation(-zAngle);
        if(!SideThrusterRight.isPlaying) {
            SideThrusterRight.Play();
        }
    }

    void StopRotation()
    {
        SideThrusterLeft.Stop();
        SideThrusterRight.Stop();
    }
}

