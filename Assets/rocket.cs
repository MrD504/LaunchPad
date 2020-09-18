using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource m_ThrustSound;
    bool thrustOn = false;

    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float mainThrust = 800f;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        m_ThrustSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter (Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("special friend");
                break;
            case "Fuel":
                print("Refueled");
                break;
            case "Finish":
                print("You win");
                break;
            default:
                print("time to die");
                break;
        }
    }
    private void Thrust()
    {
        // can thrust while rotating
        if (Input.GetKey(KeyCode.Space))
        {
            thrustOn = true;
            float thrustThisFrame = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!m_ThrustSound.isPlaying)
            {
                m_ThrustSound.Play();
            }
        }
        else
        {
            thrustOn = false;
            m_ThrustSound.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // freeze normal physics control of rotation


        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (!thrustOn)
        {
            rotationThisFrame = rotationThisFrame / 2;
        }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * rotationThisFrame);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(-(Vector3.forward * rotationThisFrame));
            }
        

        rigidBody.freezeRotation = false; // resume normal physics control of rotation
    }

   
}
