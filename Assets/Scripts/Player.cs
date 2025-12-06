using UnityEngine;
using System;

//What does this need to share as a singleton?
//  - Distance from starting platform (score).
//  - Health.
//  - Fuel.

//Notes:
//  - Input.GetKey is for an input held down, while Input.GetKeyDown is for just when pressed.



public class Player : MonoBehaviour
{
    //Initial variables:

    private Rigidbody rb;
    Vector3 P1accel = new Vector3(0.0f, 0.0f, 0.0f);     //XYZ values are how far each coordinate will update each frame.
    Vector3 P2accel = new Vector3(0.0f, 0.0f, 0.0f);
    float altitude = 0.0f;
    float HP = 100.0f;
    public float maxVelocity = 12.0f;
    float signVelocity = 0.0f;                            //Will equal the current velocity of the ship, but negative if ship is going down.
    float lastSignVelocity = 0.0f;                        //Will always equal what highestSpd was last frame.
    public float acceleration = 3.0f;
    //float deceleration = 0.00025f;
    float gravity = -2.0f;                       //The initial low-gravity value.
                                                 //(The Moon's gravity is 1.62 m/s^2)

    //Subscribers:
    private void OnEnable()
    {
        GameManager.Instance.AltitudeEvent += GetAltitude;
        GameManager.Instance.PlayerSpdChanged += GetSpd;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();                 //Initialize rigidbody variable.
        SetGravity(gravity);                     //Set the initial gravity.
    }
    
    //Update is called once per frame with the rigidbody physics:
    void Update()
    {
        if(HP > 0.0f){
            
            //Player 1 input:   
            if (Input.GetKey(KeyCode.W))
            {
                P1accel.y = -acceleration;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                P1accel.y = acceleration;
            }
            else
            {
                P1accel.y = 0;
            }

            if (Input.GetKey(KeyCode.A))
            {
                P1accel.x = acceleration;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                P1accel.x = -acceleration;
            }
            else
            {
                P1accel.x = 0;
            }

            //Player 2 input:
            if (Input.GetKey(KeyCode.UpArrow))
            {
                P2accel.y = -acceleration;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                P2accel.y = acceleration;
            }
            else
            {
                P2accel.y = 0;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                P2accel.x = acceleration;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                P2accel.x = -acceleration;
            }
            else
            {
                P2accel.x = 0;
            }
        }
        else
        {
            HP = 0.0f;
        }
    }

    //Handling player physics & movement. Is called once per frame with the rigidbody physics:
    void FixedUpdate()
    {
        //Update player position, according to user input and gravity:
        rb.AddForce(P1accel + P2accel);
        altitude = rb.position.y / 1000.0f;
        //Debug.Log("P1spd = " + P1spd + ", P2spd = " + P2spd);

        //Managing speed values:
        SpdCheck();

        //Make sure the player never moves along the z-axis:
        Vector3 onZ = rb.position;
        onZ.z = 0.0f;
        rb.MovePosition(onZ);

        //Update value used for collision damage:
        signVelocity = rb.linearVelocity.magnitude * Mathf.Sign(rb.linearVelocity.y); //Return a negative value if ship is going down.
        Debug.Log("Current velocity: " + signVelocity);

        //Make the camera zoom out futher as the player gets frather from the starting area:
        //GameManager.Instance.target = rb.transform;
        ZoomCamera(rb.position.y);

        //Make gravity weaker with elevation:
        SetGravity(gravity);

        //Debug statements:
        //Debug.Log("Player pos: (" + rb.position.x + ", " + rb.position.y + ", " + rb.position.z + ")");
        //Debug.Log("Gravity = " + Physics.gravity);
        //Debug.Log(P1spd);
    }

    //Handling collisions:
    void OnCollisionEnter(Collision collision)
    {
        //Take damage:
        if(Math.Abs(lastSignVelocity) > acceleration)
        {
            HP -= Math.Abs(lastSignVelocity) * acceleration;
        }

        if(HP < 0.0f)
        {
            HP = 0.0f;
        }
        GameManager.Instance.playerHP = HP;
        
        //Decrease the current speed:
        

        //Log the name of the object that was collided with:
        //Debug.Log("Collided with: " + collision.gameObject.name);
    }

    //Happens last in a frame:
    void LateUpdate()
    {
        lastSignVelocity = signVelocity;
        //Debug.Log("lastHighestSpd = " + lastHighestSpd);
    }

    //Parameters don't effect the original variables in C#!!
    void SpdCheck()
    {
        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
        }
    }

    //Get the player's current transform:
    Transform getTransform()
    {
        return rb.transform;
    }

    //Get the player's total spd:
    float GetSpd()
    {
        return lastSignVelocity;
    }

    //Get the player's current altitude (score):
    float GetAltitude()
    {
        return altitude;
    }

    //Change the rigidbody's gravity value:
    void SetGravity(float _gravity)
    {   
        gravity = _gravity;
        
        float offsetGravity = gravity;// + altitude * 10.0f;

        if(offsetGravity > 0.0f)
        {
            offsetGravity = 0.0f;
        }
        Physics.gravity = new Vector3(0, offsetGravity, 0);
        //Debug.Log("Physics.gravity = " + Physics.gravity);
    }

    //Make camera zoom out as the rocket travels higher:
    void ZoomCamera(float _playerY)
    {
        GameManager.Instance.transform.position = new Vector3(
            GameManager.Instance.transform.position.x, 
            GameManager.Instance.transform.position.y, 
            -10f - _playerY / 10
        );
    }
}
