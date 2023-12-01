using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    private AudioManager audioManager;

    public bool isPaused = false; // ���� ����

    public GameObject gameMenu;
    public GameObject colorPicker;

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
    }

    void StartGame()
    {
        Time.timeScale = 1f;  // ������ �ð� �帧 �簳
        isPaused = false;
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
}
