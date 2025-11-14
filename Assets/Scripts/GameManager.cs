using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    //Variables:
    public static GameManager instance;                //This is used for the singleton.
    //public Action<int> ElevationChange;                //Service for handling when player's height changes:

    //Method for being a singleton:
    public static GameManager Instance {
        get {
            if (instance == null) {
                GameObject obj = new GameObject("GameManager");
                instance = obj.AddComponent<GameManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
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
}
