﻿using UnityEngine;
using UnityEngine.SceneManagement;
public class rocket : MonoBehaviour
{
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip winJingle;
    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float mainThrust = 800f;

    
    Rigidbody rigidBody;
    AudioSource audioSource = default;
    
    bool thrustOn = false;

    // Start is called before the first frame update

    enum State { Alive, Dying, Transceding };
    State state = State.Alive;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter (Collision collision)
    {
        if (state != State.Alive) { return; } // ignore collsions when dead
        audioSource.Stop(); // stop all sounds before playing next sound

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("special friend");
                break;
            case "Finish":
                state = State.Transceding;
                Invoke("LoadNextScene", 1f);
                audioSource.PlayOneShot(winJingle);
                break;
            default:
                audioSource.PlayOneShot(hit);
                state = State.Dying;
                Invoke("LoadNextScene", 3f);
                print("time to die");
                break;
        }
    }

    private void LoadNextScene()
    {
        if (state == State.Dying)
        {
            SceneManager.LoadScene(0); // allow for more than 2 levels
        } else
        {
            SceneManager.LoadScene(1);
        }
    }

    private void RespondToThrustInput()
    {
        // can thrust while rotating
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            thrustOn = false;
        }
    }

    private void ApplyThrust()
    {
        thrustOn = true;
        float thrustThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // freeze normal physics control of rotation


        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (!thrustOn)
        {
            rotationThisFrame = rotationThisFrame / 4;
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
