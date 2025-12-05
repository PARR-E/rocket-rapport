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
    Vector3 P1spd = new Vector3(0.0f, 0.0f, 0.0f);     //XYZ values are how far each coordinate will update each frame.
    Vector3 P2spd = new Vector3(0.0f, 0.0f, 0.0f);
    float altitude = 0.0f;
    float HP = 100.0f;
    float spdMax = 0.25f;
    float highestSpd = 0.0f;
    float lastHighestSpd = 0.0f;
    float acceleration = 0.00075f;
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
        //Player 1 input:   
        if (Input.GetKey(KeyCode.W))
        {
            P1spd.y -= acceleration;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            P1spd.y += acceleration;
        }
        else
        {
            //P1spd.y -= deceleration;
        }

        if (Input.GetKey(KeyCode.A))
        {
            P1spd.x += acceleration;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            P1spd.x -= acceleration;
        }
        else
        {
            //P1spd.x -= deceleration;
        }

        //Player 2 input:
        if (Input.GetKey(KeyCode.UpArrow))
        {
            P2spd.y -= acceleration;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            P2spd.y += acceleration;
        }
        else
        {
            //P2spd.y -= deceleration;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            P2spd.x += acceleration;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            P2spd.x -= acceleration;
        }
        else
        {
            //P2spd.x -= deceleration;
        }
    }

    //Handling player physics & movement. Is called once per frame with the rigidbody physics:
    void FixedUpdate()
    {
        //Managing speed values:
        SpdCheck();

        //Update player position, according to user input and gravity:
        rb.AddForce(P1spd * 10 + P2spd * 10);
        altitude = rb.position.y / 1000.0f;
        Debug.Log("P1spd = " + P1spd + ", P2spd = " + P2spd);

        //Make sure the player never moves along the z-axis:
        Vector3 onZ = rb.position;
        onZ.z = 0.0f;
        rb.MovePosition(onZ);

        //Update value used for collision damage:
        highestSpd = Mathf.Max(Math.Abs(P1spd.x), Math.Abs(P1spd.y)) + Mathf.Max(Math.Abs(P2spd.x), Math.Abs(P2spd.y));

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
        if(lastHighestSpd > 0.000001)
        {
            HP -= lastHighestSpd * 10;
        }
        
        GameManager.Instance.playerHP = HP;
        
        //Decrease the current speed:
        P1spd = P1spd * 0.0f; //P1spd / 3;
        P2spd = P2spd * 0.0f; //P2spd / 3;

        //Log the name of the object that was collided with:
        //Debug.Log("Collided with: " + collision.gameObject.name);
    }

    //Happens last in a frame:
    void LateUpdate()
    {
        lastHighestSpd = highestSpd;
        //Debug.Log("lastHighestSpd = " + lastHighestSpd);
    }

    //Parameters don't effect the original variables in C#!!
    void SpdCheck()
    {   
        //Player 1:
        if (P1spd.x > spdMax)
        {
            P1spd.x = spdMax;
        }
        if (P1spd.y > spdMax)
        {
            P1spd.y = spdMax;
        }
        if (P1spd.x < -spdMax)
        {
            P1spd.x = -spdMax;
        }
        if (P1spd.y < -spdMax)
        {
            P1spd.y = -spdMax;
        }

        //Player 2:
        if (P2spd.x > spdMax)
        {
            P2spd.x = spdMax;
        }
        if (P2spd.y > spdMax)
        {
            P2spd.y = spdMax;
        }
        if (P2spd.x < -spdMax)
        {
            P2spd.x = -spdMax;
        }
        if (P2spd.y < -spdMax)
        {
            P2spd.y = -spdMax;
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
        return lastHighestSpd;
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
