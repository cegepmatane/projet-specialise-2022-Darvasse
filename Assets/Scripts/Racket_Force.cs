using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket_Force : MonoBehaviour
{
    [SerializeField]
    public Rigidbody rb;

    [SerializeField]
    private Vector3 lastFrameVelocity;

    [SerializeField]
    private Vector3 currentFrameVelocity;

    [SerializeField]
    private Vector3 lastFramePosition;

    [SerializeField]
    private Vector3 currentFramePosition;

    [SerializeField]
    private float force;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastFramePosition = new Vector3(0, 0, 0);
        currentFramePosition = transform.position;
        lastFrameVelocity = new Vector3(0, 0, 0);
        currentFrameVelocity = new Vector3(0, 0, 0);
        force = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastFramePosition = currentFramePosition;
        currentFramePosition = transform.position;
        lastFrameVelocity = currentFrameVelocity;
        currentFrameVelocity = (currentFramePosition - lastFramePosition) / Time.fixedDeltaTime;
        force = rb.mass * ((currentFrameVelocity.magnitude - lastFrameVelocity.magnitude) / Time.fixedDeltaTime);
        
    }

    public float getForce()
    {
        return force;
    }

}
