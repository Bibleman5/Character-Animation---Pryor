using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenControls : MonoBehaviour
{
    // Variables

    // Access Unity APIs for components
    public CharacterController controller;
    public Animator anim;

    // Assign an audio clip file and access the AudioSource API
    public AudioClip runningSound;
    private AudioSource audioSource;

    // Values for rotation and running speeds
    public float runningSpeed = 4.0f;
    public float rotationSpeed = 100.0f;

    // States and settings
    private float runInput;
    private float rotateInput;
    private bool eating = false;
    private bool turningHead = false;

    // Declare a 3D vector for moving
    public Vector3 moveDir;

    // Starting Function
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!turningHead && !eating)
        {
            // Assign player input from Project Settings to variables
            runInput = Input.GetAxis("Vertical");
            rotateInput = Input.GetAxis("Horizontal");
        } else {
            runInput = 0;
            rotateInput = 0;
        }
        
        // Check to see if eating or turning head keys are pressed
        CheckEat();
        CheckTurningHead();

        // Set moveDir to new Vector3 based on player input
        moveDir = new Vector3(0, 0, runInput * runningSpeed);
        // Update the character's direction based on the game world and player input
        moveDir = transform.TransformDirection(moveDir);
        // Move the character using the controller in the direction and new position set earlier
        controller.Move(moveDir * Time.deltaTime);

        // Update character rotation
        transform.Rotate(0f, rotateInput * rotationSpeed * Time.deltaTime, 0f);

        // Update animations and sound effects based on player input values
        Effects();
    }

    void CheckEat()
    {
        if (Input.GetKey(KeyCode.Z) && !turningHead)
        {
            eating = true;
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        } else {
            eating = false;
        }
    }

    void CheckTurningHead()
    {
        if (Input.GetKey(KeyCode.Space) && !eating)
        {
            turningHead = true;
        } else {
            turningHead = false;
        }
    }

    void Effects()
    {
        if (runInput != 0 && !eating && !turningHead)
        {
            anim.SetBool("Run Forward", true);
            if (audioSource != null && !audioSource.isPlaying && controller.isGrounded)
            {
                audioSource.clip = runningSound;
                audioSource.Play();
            }
        } else {
            anim.SetBool("Run Forward", false);
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        if (eating)
        {
            anim.SetBool("Eat", true);
        } else {
            anim.SetBool("Eat", false);
        }

        if (turningHead)
        {
            anim.SetBool("Turn Head", true);
        } else {
            anim.SetBool("Turn Head", false);
        }
    }
} 