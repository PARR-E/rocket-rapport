using UnityEngine;
using TMPro;
using System;

public class GameUIController : MonoBehaviour
{
    //Subscribers:
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    //Start() is guaranteed to run after all Awake() calls, so GameManager.Instance exists.
    void Start()

    {
        GameManager.Instance.UpdateScore += UpdateAltitude;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Change the UI:
    void UpdateAltitude(float score)
    {
        //Update the altitude UI:
        TextMeshProUGUI textComponent;

        Transform child2 = transform.GetChild(1);
        textComponent = child2.GetComponent<TextMeshProUGUI>();

        textComponent.text = score.ToString("F5") + " AU";          //F5 rounds the number to 5 decimal places.
    }
}
