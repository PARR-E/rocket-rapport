using UnityEngine;


public class Obstacle : MonoBehaviour
{
    //Initial variables:
    private Rigidbody rb;
    float playerY = 0.0f;
    public float bottomEdgeOffset = 1.0f;
    float rotationSpd = 0.01f;
    public float maxRotationSpd = 15.0f;
    float sizeMax = 1.0f;
    bool sizeChanged = false;
    bool forceChanged = false;
    float forceMaxX;
    float forceMaxY;
    float forceX;
    float forceY;

    //Subscribers:
    private void OnEnable()
    {
        GameManager.Instance.scoreChanged += UpdatePlayerY;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();                         //Initialize rigidbody variable.
        rb.constraints = RigidbodyConstraints.FreezePositionZ;  //Make sure the obstacles never move along the z-axis.

        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);

        //Set random rotation speed:
        rotationSpd = Random.Range(-maxRotationSpd, maxRotationSpd);

        //Debug.Log("Player Y is " + playerY);

        Vector3 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(x, playerY + 1.5f, Mathf.Abs(Camera.main.transform.position.z)));

        rb.position = worldPos;
    }

    // Update is called once per frame
    void Update()
    {   
        //Increase size here because this is where playerY is gotten:
        if(!sizeChanged){
            sizeMax = 1.5f + playerY / 90.0f;
            //Debug.Log("sizeMax = " + sizeMax);
            if (sizeMax > 20.0f)
            {
                sizeMax = 20.0f;
                //Debug.Log("Max possible asteroid size hit");
            }
            float size = Random.Range(1f, sizeMax);
            transform.localScale = new Vector3(size, size, size);

            sizeChanged = true;
        }
        
        //Destroy this obstacle when off screen:
        float zDist = Mathf.Abs(Camera.main.transform.position.z);
        float padding = 0.0f;       //Increase this to increase the height of the despawn line.
        float bottomEdge = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, zDist)).y;
        bottomEdge -= sizeMax * 0.5f + padding;

        if(rb.position.y < bottomEdge || Mathf.Abs(rb.position.z) > 0.5 || Mathf.Abs(rb.position.x) > 105)
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        //Add force:
        if(!forceChanged){
            forceMaxX = 0.005f + playerY / 1500;
            forceMaxY = 0.01f + playerY / 2000;

            if(forceMaxX > 0.25f)
            {
                forceMaxX = 0.25f;
            }
            if(forceMaxY > 0.02)
            {
                forceMaxY = 0.02f;
                //Debug.Log("ForceMaxY is max");
            }

            forceX = Random.Range(-0.02f, 0.02f);
            forceY = Random.Range(0.0005f, -forceMaxY);
            rb.AddForce(new Vector3(forceX, forceY, 0.0f));

            //Add rotation too:
            rb.AddTorque(new Vector3(0.0f, 0.0f, rotationSpd));

            forceChanged = true;
        }
    }

    //Update the PlayerY variable:
    void UpdatePlayerY(float playerScore)
    {   
        //Altitude equals player position y divided by 1000, so need to turn that back into a y-value:
        playerY = playerScore * 1000;
    }

    //Be sure to unsubscribe on a scene reload:
    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.scoreChanged -= UpdatePlayerY;
        }
    }
}
