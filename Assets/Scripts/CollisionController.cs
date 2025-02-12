using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    // Let's you change the color of an object upon collision
    public bool changeColor;
    public Color myColor;

    // States of GameObjects to destroy them upon collision
    public bool destroyEnemy;
    public bool destroyCollectibles;

    // Allows you to add an audio file that's played on collision
    public AudioClip collisionAudio;
    private AudioSource audioSource;

    // New variable for pushing force
    public float pushPower = 2.0f;  // Default push power for the Boximon Cyclops character

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // OnCollisionEnter is called when the object collides with another object
    void OnCollisionEnter(Collision other)
    {
        // If changeColor is true, change the color of the GameObject upon collision
        if (changeColor == true)
        {
            gameObject.GetComponent<Renderer>().material.color = myColor;
        }

        // If the audio source exists and is not already playing, play the collision audio
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(collisionAudio, 0.5f);
        }

        // If destroyEnemy is true and the collided object has the tag "Enemy"
        // OR if destroyCollectibles is true and the collided object has the tag "Collectible"
        if ((destroyEnemy && other.gameObject.tag == "Enemy") || 
            (destroyCollectibles && other.gameObject.tag == "Collectible"))
        {
            Destroy(other.gameObject);
        }
    }

    // New function for handling Character Controller collisions
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Get the Rigidbody of the object the character collided with
        Rigidbody body = hit.collider.attachedRigidbody;

        // If no Rigidbody or if it is Kinematic, do nothing
        if (body == null || body.isKinematic)
        {
            return;
        }

        // Don't push ground or platform GameObjects below character
        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        // Calculate push direction from move direction, only along x and z axes (no vertical pushing)
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If the collided object has the "Object" tag, apply the push force
        if (hit.gameObject.tag == "Object")
        {
            body.velocity = pushDir * pushPower;
        }

        // Play the collision sound effect if audioSource exists and is not already playing
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(collisionAudio, 0.5f);
        }

        // Destroy objects tagged as "Enemy" or "Collectible"
        if (destroyEnemy == true && hit.gameObject.tag == "Enemy" || 
            destroyCollectibles == true && hit.gameObject.tag == "Collectible")
        {
            Destroy(hit.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Any update logic for your character controller can go here if necessary
    }
}