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
        rb.AddForce(new Vector3(-0.2f, -0.01f, 0.0f));
    }

    void UpdatePlayerY(float playerScore)
    {   
        //Altitude equals player position y divided by 1000, so need to turn that back into a y-value:
        playerY = playerScore * 1000;
    }
}
