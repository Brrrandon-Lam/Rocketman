using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] float LevelLoadDelay = 1.0f;
    [SerializeField] AudioClip failedLevel;
    [SerializeField] AudioClip passedLevel;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    // If transitioning, don't do stuff.
    bool isTransitioning = false;
    bool collisionsDisabled = false;

    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        Cheat_Keys();
    }

    void Cheat_Keys()
    {
        // Use GetKeyDown so that we don't cross multiple frames.
        if(Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("Loaded next level");
            LoadNextLevel();
        }
        if(Input.GetKeyDown(KeyCode.C)) {
            Debug.Log("Collisions toggled");
            collisionsDisabled = !collisionsDisabled;
        }
    }

    void OnCollisionEnter(Collision other) {
        // If collisions are disabled or we are transitioning, return.
        if(isTransitioning || collisionsDisabled) { return; }
        else {
            switch(other.gameObject.tag) {
                case "Respawn":
                    // Nothing for now
                    Debug.Log("TODO: Level Start!");
                    break;
                case "Finish":
                    // Go to the next scene or loop to start scene
                    StartCompletedSequence();
                    
                    //LoadNextLevel();
                    break;
                default:
                    // Reload the scene
                    StartCrashSequence();
                    break;
            }
        }
    }

    void StartCompletedSequence()
    {
        // Add SFX
        // Disable movement so that audio doesn't overwrite.
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        successParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(passedLevel, 0.2F);

        // Success particle

        // Invoke level loader
        Invoke("LoadNextLevel", LevelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        // Add SFX and crash particles on crash
        crashParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(failedLevel, 0.2F);
        // Remove movement
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", LevelLoadDelay);
    }

    void ReloadLevel()
    {
        // Loads the scene based on the index of the active scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadNextLevel()
    {
        int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // We have 0 to n - 1 scenes, where n is the number of scenes.
        // If current + 1 = n then we need to reset to 0.
        if(CurrentSceneIndex + 1 >= SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene(0);
        }
        else {
            SceneManager.LoadScene(CurrentSceneIndex + 1);
        }

    }
}
