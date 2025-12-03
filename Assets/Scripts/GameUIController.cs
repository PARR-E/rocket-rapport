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
        GameManager.Instance.scoreChanged += UpdateAltitude;
        GameManager.Instance.healthChanged += UpdateHP;
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

        Transform child1 = transform.GetChild(1);
        textComponent = child1.GetComponent<TextMeshProUGUI>();

        textComponent.text = score.ToString("F5") + " AU";          //F5 rounds the number to 5 decimal places.
    }
    void UpdateHP(float hp)
    {
        //Update the altitude UI:
        TextMeshProUGUI textComponent;

        Transform child3 = transform.GetChild(3);
        textComponent = child3.GetComponent<TextMeshProUGUI>();

        textComponent.text = hp.ToString("F2") + " HP";          //F2 rounds the number to 2 decimal places.
    }
}
