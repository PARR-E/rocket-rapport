using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameUIController : MonoBehaviour
{
    //Subscribers:
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    //Start() is guaranteed to run after all Awake() calls, so GameManager.Instance exists.
    void Start()

    {
        GameManager.Instance.changeScoreUI += UpdateAltitude;
        GameManager.Instance.changeHighScoreUI += UpdateHighestAltitude;
        GameManager.Instance.healthChanged += UpdateHP;
        GameManager.Instance.gameOver += GameOver;
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
    //Change highest score:
    void UpdateHighestAltitude(float score)
    {
        //Update the altitude UI:
        TextMeshProUGUI textComponent;

        Transform child1 = transform.GetChild(5);
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

    void GameOver(float score)
    {
        //Update the game over UI:
        TextMeshProUGUI textComponent;

        Transform childGameOver = transform.GetChild(6);
        textComponent = childGameOver.GetComponent<TextMeshProUGUI>();
        textComponent.gameObject.SetActive(true);

        childGameOver = transform.GetChild(7);
        childGameOver.gameObject.SetActive(true);

        childGameOver = transform.GetChild(8);
        childGameOver.gameObject.SetActive(true);
    }

    //Be sure to unsubscribe on a scene reload:
    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.changeScoreUI -= UpdateAltitude;
            GameManager.Instance.changeHighScoreUI -= UpdateHighestAltitude;
            GameManager.Instance.healthChanged -= UpdateHP;
            GameManager.Instance.gameOver -= GameOver;
        }
    }
}
