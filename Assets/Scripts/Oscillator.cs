using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
     // Make something move in a sine wave.
    // Start is called before the first frame update
    [SerializeField] float Amplitude = 1.0f;
    [SerializeField] float Frequency = 1.0f;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Oscillates in a sine wave on the X axis, modified by Frequency and Amplitude).
        Vector3 offset = new Vector3(0.0f, (Mathf.Sin(Time.time * Frequency) + 1) / 2, 0.0f) * Amplitude;
        transform.position = startPosition + offset;
    }
}
