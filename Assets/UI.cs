using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject playerUI;
    public GameObject GameOverUI;
    public GameObject GameFinishedUI;
    public GameObject StartGameUI;


    // Start is called before the first frame update
    void Start()
    {
        //at start freeze time and set UI
        Time.timeScale = 0f;
        playerUI.SetActive(false);
        GameOverUI.SetActive(false);
        GameFinishedUI.SetActive(false);
        StartGameUI.SetActive(true);
    }
    //Quit Button
    public void Quit()
    {
        Quit();
    }
    //Start Button
    public void start()
    {
        Time.timeScale = 1f;
        playerUI.SetActive(true);
        StartGameUI.SetActive(false);
        GameFinishedUI.SetActive(false);
    }
    //restart button
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    //function to call when player dies
    public void GameOver()
    {
        playerUI.SetActive(false);
        GameOverUI.SetActive(true);
        GameFinishedUI.SetActive(false);
        Time.timeScale = 0f;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }

    public void GameWon()
    {
        playerUI.SetActive(false);
        GameOverUI.SetActive(false);
        GameFinishedUI.SetActive(true);
        Time.timeScale = 0f;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }
}
