using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    //Subscribers:
    private void OnEnable()
    {
        GameManager.Instance.moveBG += updateBG;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    //Make the background always move with the camera:
    void updateBG(float newY, float newZ)
    {
        transform.position = new Vector3(0.0f, newY, 140.0f - newZ);
    }
}
