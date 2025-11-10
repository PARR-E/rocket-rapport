using UnityEngine;

//What does this need to share as a singleton?
//  - Distance from starting platform (score).
//  - Health.
//  - Fuel.


public class Player : MonoBehaviour
{
    //Initial variables:
    private Rigidbody rb;
    Vector3 P1spd = new Vector3(0.0f , 0.0f, 0.0f);
    Vector3 P2spd = new Vector3(0.0f , 0.0f, 0.0f);
    float gravity = 0.10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Player input:


        //Update player position, according to user input and gravity:
        P2spd.y -= gravity;
        rb.position += P1spd + P2spd;

        rb.MovePosition(rb.position);

        //On collision enter, change current position to last frame's position:
        
    }
}
