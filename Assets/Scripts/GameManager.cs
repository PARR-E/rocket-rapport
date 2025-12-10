using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    //Variables:
    public Transform target;                                //Equals the player.
    public float smoothTime = 0.01f;
    public Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);  //Distance between camera and player.
    
    private Vector3 velocity = Vector3.zero;
    public float playerScore = 0.0f;
    public float GameHighScore { get; set; } = 0;           //Will hold the current high score from the database.

    public float playerHP = 100.0f;
    public float playerSpd = 0.0f;

    public static GameManager instance;                //This is used for the singleton.
    //public GameObject obstaclePrefab;                   //Will point to the obstacle prefab in the Unity Inspector.

    public event Func<float> AltitudeEvent;
    public event Func<float> PlayerSpdChanged;
    
    public Action<float> scoreChanged;
    public Action<float> changeScoreUI;
    public Action<float> changeHighScoreUI;
    public Action<float> healthChanged;
    public Action<float> gameOver;
    public Action<float, float> moveBG;
    

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

        Application.targetFrameRate = 60;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = FindObjectOfType<Player>().transform;

        //Initial database stuff:
        PlayerData player = DatabaseManager.Instance.LoadPlayerData();  
        if (player != null)
        {
            GameHighScore = player.HighScore;
        }
        else
        {
            GameHighScore = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        healthChanged?.Invoke(playerHP);
        scoreChanged?.Invoke(playerScore);
        
        //Handle Game Over:
        if(playerHP <= 0.0f)
        {
            //Debug.Log("GAME OVER");
            gameOver?.Invoke(playerScore);

            //Save the high score!
            DatabaseManager.Instance.SavePlayerData("Player", GameHighScore);
        }
        //While player is alive:
        else
        {
            //Update high score:
            if(playerScore > GameHighScore)
            {
                GameHighScore = playerScore;
            }
            
            //Update the UI:
            playerScore = AltitudeEvent?.Invoke() ?? 0f;    //Use the null-coalescing operator (??) to supply a fallback float if nothing is returned.
            playerSpd = PlayerSpdChanged?.Invoke() ?? 0f;
            
            changeScoreUI?.Invoke(playerScore);
            changeHighScoreUI?.Invoke(GameHighScore);
        }
        
        //Debug.Log("Score = " + score);
    }

    //Have the camera trail behind the player:
    void FixedUpdate()
    {   
        Vector3 targetPos = new Vector3(transform.position.x, target.position.y + playerSpd / 2, transform.position.z) + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime,
            100.0f          //Max camera follow speed.
        );
    }
}
