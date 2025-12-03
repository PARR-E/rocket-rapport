using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    //Variables:
    public Transform target;                                //Equals the player.
    public float smoothTime = 0.2f;
    public Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);  //Distance between camera and player.
    
    private Vector3 velocity = Vector3.zero;
    public float playerScore = 0.0f;
    public float playerHP = 100.0f;

    public static GameManager instance;                //This is used for the singleton.

    public event Func<float> AltitudeEvent;
    public Action<float> scoreChanged;
    public Action<float> healthChanged;

    //Method for being a singleton:
    public static GameManager Instance {
        get { return instance; }
    }
    //This is also needed to be a singleton:
    private void Awake()
    {   
        //Dr. Zheng's version:
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {   
        //Update the UI:
        playerScore = AltitudeEvent?.Invoke() ?? 0f;    //Use the null-coalescing operator (??) to supply a fallback float if nothing is returned.
        scoreChanged?.Invoke(playerScore);
        healthChanged?.Invoke(playerHP);
        
        //Debug.Log("Score = " + score);
    }

    //Have the camera trail behind the player:
    void FixedUpdate()
    {   
        Vector3 targetPos = new Vector3(transform.position.x, target.position.y, transform.position.z) + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }
}
