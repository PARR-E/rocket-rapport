using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    //Subscribers:
    private void OnEnable()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.moveBG += UpdateBG;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, -0.05f * Time.deltaTime, 0.0f));
    }

    //Make the background always move with the camera:
    void UpdateBG(float newY)
    {
        transform.position = new Vector3(0.0f, newY, 0.0f);
        Debug.Log("BG should be moving");
    }

    //Cleanly unsubscribe:
    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.moveBG -= UpdateBG;
    }
}
