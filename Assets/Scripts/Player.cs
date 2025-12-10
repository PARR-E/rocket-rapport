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
    public GameObject obstaclePrefab;                   //Will point to the obstacle prefab in the Unity Inspector.
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
    float initialGravity = -2.0f;                       //The initial low-gravity value.
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
        rb = GetComponent<Rigidbody>();                         //Initialize rigidbody variable.
        rb.constraints = RigidbodyConstraints.FreezePositionZ;  //Make sure the obstacles never move along the z-axis.
        SetGravity();                     //Set the initial gravity.
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

        //If player is outside the safe zone, start spawning hazards:
        if(rb.position.y > 10.0)
        {    
            float upperChance = 100.0f - altitude * 250.0f;     //Increase rightmost value to increase how quickly difficulty increases.
            if(upperChance < 10.0f)
            {
                upperChance = 10.0f;
            }
            //Debug.Log("upperChance = " + upperChance);

            float asteroidChance = UnityEngine.Random.Range(0f, upperChance);
            //Spawn an asteroid:
            if(asteroidChance < 1.0f && HP > 0.0f)
            {
                GameObject obstacle = Instantiate<GameObject>(obstaclePrefab);
            }
        }
    }

    //Handling player physics & movement. Is called once per frame with the rigidbody physics:
    void FixedUpdate()
    {
        //Update player position, according to user input and gravity:
        if(HP > 0.0f)
        {
            rb.AddForce(P1accel + P2accel);
            altitude = rb.position.y / 1000.0f;
            //Managing speed values:
            SpdCheck();
        }
        //If player HP depleted, remove model:
        else
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        

        //Update value used for collision damage:
        signVelocity = rb.linearVelocity.magnitude * Mathf.Sign(rb.linearVelocity.y); //Return a negative value if ship is going down.
        //Debug.Log("Current velocity: " + signVelocity);

        //Make the camera zoom out futher as the player gets frather from the starting area:
        //GameManager.Instance.target = rb.transform;
        
        //Zoom out the camera, & if the player is out of bounds, subtract HP.
        float offScreen = ZoomCamera(rb.position.y);
        if (offScreen != 0.0f)
        {   
            HP -= offScreen / 2;
            HandleDamage();
            //Debug.Log("Out of bounds by " + offScreen);
        }

        //Make gravity weaker with elevation:
        SetGravity();

        //Debug statements:
        Debug.Log("Player pos: (" + rb.position.x + ", " + rb.position.y + ", " + rb.position.z + ")");
        //Debug.Log("Gravity = " + Physics.gravity);
        //Debug.Log(rb.position);
    }

    //Handling collisions:
    void OnCollisionEnter(Collision collision)
    {
        //Take damage:
        if(Math.Abs(lastSignVelocity) > 1.0f)
        {
            HP -= Math.Abs(lastSignVelocity) * acceleration * 0.5f;
        }

        HandleDamage();
        
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
    void SetGravity()
    {     
        float offsetGravity = initialGravity + altitude * 4.0f;

        if(offsetGravity > 0.0f)
        {
            offsetGravity = 0.0f;
        }
        Physics.gravity = new Vector3(0, offsetGravity, 0);
        //Debug.Log("Physics.gravity = " + Physics.gravity);
    }

    //Update the Hp variable in GameManager:
    void HandleDamage()
    {
        if(HP < 0.0f)
        {
            HP = 0.0f;
        }
        GameManager.Instance.playerHP = HP;
    }

    //Structure used for finding how far offscreen the player is:
    public struct OffscreenDistance
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    //Make camera zoom out as the rocket travels higher:
    float ZoomCamera(float _playerY)
    {
        //Have camera zoom out as the rocket gains more distance:
        float offsetCamera = -10f - _playerY / 15;
        float offsetCameraLimit = -100.0f;
        if(offsetCamera < offsetCameraLimit)
        {
            offsetCamera = offsetCameraLimit;
        }
        //Debug.Log("offset = " + offsetCamera);
        
        //Apply camera zoom:
        GameManager.Instance.transform.position = new Vector3(
            GameManager.Instance.transform.position.x, 
            GameManager.Instance.transform.position.y, 
            offsetCamera
        );

        //Update background accordingly:
        GameManager.Instance.moveBG?.Invoke(GameManager.Instance.transform.position.y, GameManager.Instance.transform.position.z);

        //Variable for finding ig offscreen:
        OffscreenDistance d = new OffscreenDistance();
        Vector3 vp = Camera.main.WorldToViewportPoint(rb.position);

        //Return a nonzero value if player is out of bounds:
        d.left   = vp.x < 0f ? -vp.x : 0f;
        d.right  = vp.x > 1f ? vp.x - 1f : 0f;
        d.bottom = vp.y < 0f ? -vp.y : 0f;
        d.top    = vp.y > 1f ? vp.y - 1f : 0f;

        if(d.right > 0.0f)
        {
            return d.right;
        }
        else if(d.left > 0.0f)
        {
            return d.left;
        }
        else if(d.bottom > 0.0f)
        {
            return d.bottom;
        }
        else
        {
            return 0.0f;
        }
    }

    //Be sure to unsubscribe on a scene reload:
    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AltitudeEvent -= GetAltitude;
            GameManager.Instance.PlayerSpdChanged -= GetSpd;
        }
    }
}
