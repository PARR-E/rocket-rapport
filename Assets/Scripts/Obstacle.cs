using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //Initial variables:

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();                 //Initialize rigidbody variable.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.AddForce(new Vector3(-0.2f, -0.1f, 0.0f));
    }
}
