using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private AudioManager audioManager;

    public bool isPaused = false; // 게임 상태

    public GameObject stop;
    public GameObject go;
    
    public GameObject gameMenu;
    public GameObject colorPicker;

    public GameObject endGameMenu;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    
    public void GameStop()
    {
        audioManager.ButtonSound();
        if (isPaused)
        {
            StartGame();  // 게임 다시 시작
        }
        else
        {
            StopGame();  // 게임 일시 정지
        }
    }

    void StopGame()
    {
        Time.timeScale = 0f;  // 게임의 시간 흐름 멈춤
        isPaused = true;
        stop.SetActive(false);
        go.SetActive(true);
    }

    void StartGame()
    {
        Time.timeScale = 1f;  // 게임의 시간 흐름 재개
        isPaused = false;
        stop.SetActive(true);
        go.SetActive(false);
    }

    public void OnGameMenu()
    {
        audioManager.ButtonSound();
        gameMenu.SetActive(!gameMenu.activeSelf);
    }

    public void OnColorPicker()
    {
        audioManager.ButtonSound();
        colorPicker.SetActive(!colorPicker.activeSelf);
    }

    public void EndGameMenu()
    {
        endGameMenu.SetActive(!endGameMenu.activeSelf);
    }
    public void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
