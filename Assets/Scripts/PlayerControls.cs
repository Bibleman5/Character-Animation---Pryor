using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Variables
    public CharacterController controller;
    public Animator anim;
    public AudioClip runningSound;
    private AudioSource audioSource;
    public AudioClip stepSound; // Sound effect for footsteps
    private float stepTimer = 0.0f; // Timer to manage step sound frequency

    public float runningSpeed = 4.0f;
    public float rotationSpeed = 100.0f;
    public float jumpHeight = 6.0f;
    public float stepInterval = 0.5f; // Time interval between footstep sounds

    private float jumpInput;
    private float runInput;
    private float rotateInput;

    public Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        // Get the CharacterController, Animator, and AudioSource components
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input values for movement and rotation
        runInput = Input.GetAxis("Vertical");  // Forward/Backward input
        rotateInput = Input.GetAxis("Horizontal");  // Left/Right input

        // Call jump checking function
        CheckJump();

        // Set moveDir to new Vector3 based on input values
        moveDir = new Vector3(0, jumpInput * jumpHeight, runInput * runningSpeed);

        // Transform the direction to match the world space
        moveDir = transform.TransformDirection(moveDir);

        // Move the character based on direction and deltaTime for frame-independent movement
        controller.Move(moveDir * Time.deltaTime);

        // Rotate the character based on Horizontal input
        transform.Rotate(0f, rotateInput * rotationSpeed * Time.deltaTime, 0f);

        // Call function to handle animations and sound effects
        Effects();
    }

    // Check if the player is pressing space to jump
    void CheckJump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            jumpInput = 1;

            // Stop the running sound if it's playing
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        // Only allow jumpInput when grounded
        if (controller.isGrounded)
        {
            jumpInput = 0;
        }
    }

    // Manage animations and sound effects
    void Effects()
    {
        // Handle running animation and sound
        if (runInput != 0)
        {
            anim.SetBool("Run Forward", true);

            // Play running sound if not already playing
            if (audioSource != null && !audioSource.isPlaying && controller.isGrounded)
            {
                audioSource.clip = runningSound;
                audioSource.Play();
            }

            // Handle footstep sound effect
            if (controller.isGrounded && stepTimer <= 0f) // Only play when grounded
            {
                audioSource.PlayOneShot(stepSound);
                stepTimer = stepInterval; // Reset timer for next step sound
            }
        }
        else
        {
            anim.SetBool("Run Forward", false);

            // Stop running sound if playing
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        // Update step timer
        if (stepTimer > 0f)
        {
            stepTimer -= Time.deltaTime;
        }
    }
}
