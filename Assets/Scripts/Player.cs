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
    float spdMax = 1.0f;
    float acceleration = 0.0025f;
    float deceleration = 0.0005f;
    float gravity = -2.0f;                       //The initial low-gravity value.
                                                 //(The Moon's gravity is 1.62 m/s^2)

    


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
        if (Input.GetKey(KeyCode.A))
        {
            P1spd.x += acceleration;
        }
        if (Input.GetKey(KeyCode.S))
        {
            P1spd.y += acceleration;
        }
        if (Input.GetKey(KeyCode.D))
        {
            P1spd.x -= acceleration;
        }
        //Player 2 input:
        if (Input.GetKey(KeyCode.UpArrow))
        {
            P1spd.y -= acceleration;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            P1spd.x += acceleration;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            P1spd.y += acceleration;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            P1spd.x -= acceleration;
        }
    }

    //Handling player physics & movement. Is called once per frame with the rigidbody physics:
    void FixedUpdate()
    {
        //Managing speed values:
        SpdCheck(P1spd);
        SpdCheck(P2spd);


        //Update player position, according to user input and gravity:
        rb.position += P1spd + P2spd;
        rb.MovePosition(rb.position);

        //Make the camera zoom out futher as the player gets frather from the starting area:
        UpdateCamera(rb.position.y);


        //Debug statements:
        //Debug.Log("Player pos: (" + rb.position.x + ", " + rb.position.y + ", " + rb.position.z + ")");
        //Debug.Log("Gravity = " + Physics.gravity);
        //Debug.Log(P1spd);
    }

    //Handling collisions:
    void OnCollisionEnter(Collision collision)
    {
        //Decrease the current speed:
        P1spd = P1spd / 3;
        P2spd = P2spd / 3;

        //Log the name of the object that was collided with:
        Debug.Log("Collided with: " + collision.gameObject.name);
    }

    void SpdCheck(Vector3 _spd)
    {
        if (_spd.x > spdMax)
        {
            _spd.x = spdMax;
        }
        if (_spd.y > spdMax)
        {
            _spd.y = spdMax;
        }
        if (_spd.x < -spdMax)
        {
            _spd.x = -spdMax;
        }
        if (_spd.y < -spdMax)
        {
            _spd.y = -spdMax;
        }
    }

    //Change the rigidbody's gravity value:
    void SetGravity(float _gravity)
    {
        gravity = _gravity;
        Physics.gravity = new Vector3(0, gravity, 0);
    }

    //Make camera zoom out as the rocket travels higher:
    void UpdateCamera(float _playerY)
    {
        GameManager.Instance.transform.position = new Vector3(0f, 3f, -10f - _playerY / 10);
    }
}
