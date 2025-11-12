using UnityEngine;

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
    float spdMax = 4.0f;
    float acceleration = 0.02f;
    float deceleration = 0.005f;
    float gravity = -2.0f;                       //The initial low-gravity value.
                                                        //(The Moon's gravity is 1.62 m/s^2)


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();                 //Initialize rigidbody variable.
        setGravity(gravity);                     //Set the initial gravity.
    }

    // Update is called once per frame
    void Update()
    {
        //Player input:
        if (Input.GetKey(KeyCode.S))
        {
            P1spd.y += acceleration;
            Debug.Log("S pressed!");
        }

        //Decrease speed values if there is gravity:
        P1spd.x -= deceleration * 0.5f * -gravity;      //Gravity is negative!!
        P1spd.y -= deceleration * 0.5f * -gravity;

        //Managing speed values:
        if (P1spd.x > spdMax)
        {
            P1spd.x = spdMax;
        }
        if (P1spd.y > spdMax)
        {
            P1spd.y = spdMax;
        }
        if (P1spd.x < 0)
        {
            P1spd.x = 0;
        }
        if (P1spd.y < 0)
        {
            P1spd.y = 0;
        }


        //Update player position, according to user input and gravity:
        rb.position += P1spd + P2spd;
        rb.MovePosition(rb.position);

        //On collision enter, change current position to last frame's position (might not be needed):



        //Debug statements:
        //Debug.Log("Player pos: (" + rb.position.x + ", " + rb.position.y + ", " + rb.position.z + ")");
        //Debug.Log("Gravity = " + Physics.gravity);
        Debug.Log(P1spd);

    }

    //Change the rigidbody's gravity value:
    void setGravity(float _gravity)
    {
        gravity = _gravity;
        Physics.gravity = new Vector3(0, gravity, 0);
    }
}
