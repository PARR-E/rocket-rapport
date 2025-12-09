using UnityEngine;


public class Obstacle : MonoBehaviour
{
    //Initial variables:
    private Rigidbody rb;
    float playerY = 0.0f;

    

    //Subscribers:
    private void OnEnable()
    {
        GameManager.Instance.scoreChanged += UpdatePlayerY;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();                 //Initialize rigidbody variable.

        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        //Debug.Log("Player Y is " + playerY);

        Vector3 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(x, playerY + 3.0f, Mathf.Abs(Camera.main.transform.position.z)));

        rb.position = worldPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize * 2.5f)
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        float forceMaxX = 0.005f + playerY / 1500;
        float forceMaxY = 0.01f + playerY / 500;

        if(forceMaxX > 0.25f)
        {
            forceMaxX = 0.25f;
            Debug.Log("ForceMaxX is max");
        }
        if(forceMaxY > 2.5f)
        {
            forceMaxY = 2.5f;
        }

        rb.AddForce(new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(0.001f, -forceMaxY), 0.0f));

        //Make sure the obstacles never move along the z-axis:
        Vector3 onZ = rb.position;
        onZ.z = 0.0f;
        rb.MovePosition(onZ);
    }

    void UpdatePlayerY(float playerScore)
    {   
        //Altitude equals player position y divided by 1000, so need to turn that back into a y-value:
        playerY = playerScore * 1000;
    }
}
