using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    //Variables:
    public Transform target;                                //Equals the player.
    public float smoothTime = 0.2f;
    public Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);  //Distance between camera and player.
    
    private Vector3 velocity = Vector3.zero;

    public static GameManager instance;                //This is used for the singleton.

    //Method for being a singleton:
    public static GameManager Instance {
        get { return instance; }
    }
    //ChatGPT says this creates 2 instances:
    /*public static GameManager Instance {
        get {
            if (instance == null) {
                GameObject obj = new GameObject("GameManager");
                instance = obj.AddComponent<GameManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }*/
    //This is also needed to be a singleton:
    private void Awake()
    {   
        //Dr. Zheng's version:
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    //Subscribers:
    /*private void OnEnable(){
        Player.Instance.ElevationChange += UpdateCamera;        //Observer.
    }*/

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Have the camera trail behind the player:
    void FixedUpdate()
    {   
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z) + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }
}
