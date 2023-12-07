using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private AudioManager audioManager;

    public bool isPaused = false; // ���� ����

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
            StartGame();  // ���� �ٽ� ����
        }
        else
        {
            StopGame();  // ���� �Ͻ� ����
        }
    }

    void StopGame()
    {
        Time.timeScale = 0f;  // ������ �ð� �帧 ����
        isPaused = true;
        stop.SetActive(false);
        go.SetActive(true);
    }

    void StartGame()
    {
        Time.timeScale = 1f;  // ������ �ð� �帧 �簳
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
